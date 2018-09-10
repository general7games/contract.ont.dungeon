import sys
import os
import datetime
import json
import requests

import utils

def get_avm2hex():
	scriptPath = os.path.dirname(os.path.realpath(__file__))
	avm2hex = os.path.join(scriptPath, '..', 'neon', 'Avm2Hex.exe')
	if os.path.exists(avm2hex):
		return avm2hex
	return None

epoch = datetime.datetime(1970, 1, 1)
def now():
	return int((datetime.datetime.utcnow() - epoch).total_seconds())

def deploy(config, avmFilename, password, needStorage, author, email, description):

	if not config.has_key('address'):
		return 'missing address in config', None

	# check avm2hex
	avm2hex = get_avm2hex()
	if not avm2hex:
		return 'missing Avm2Hex', None

	# check avm and abi
	if not os.path.exists(avmFilename):
		return 'file not found ' + avmFilename, None
	abiFilename = os.path.splitext(avmFilename)[0] + '.abi.json'
	if not os.path.exists(abiFilename):
		return 'file not found ' + abiFilename, None

	# avm2hex
	hexFilename = os.path.splitext(avmFilename)[0] + '.hex'
	errCode = os.system(avm2hex + ' --input {0} --output {1}'.format(avmFilename, hexFilename))
	if errCode != 0:
		return 'avm2hex failed', None

	if not os.path.exists(hexFilename):
		return 'avm2hex executed successful but ' + hexFilename + ' not found'


	encPassword = utils.enc_password(password)

	name = os.path.splitext(os.path.basename(avmFilename))[0]

	script = None
	with open(hexFilename, 'rb') as f:
		script = f.read()

	version = str(now())

	# author, email, description, needStorage

	abi = None
	with open(abiFilename, 'r') as f:
		abi = json.load(f)

	(err, url) = utils.get_url(config, '/admin/deployContract')
	if err:
		return err, None

	r = requests.post(url, json={
		'name': name,
		'script': script,
		'version': version,
		'author': author,
		'email': email,
		'description': description,
		'storage': needStorage,
		'abi': abi,
		'account': {
			'address': config['address'],
			'password': encPassword
		}
	})
	content = r.json()
	if content['error'] != 0:
		return content['result'], None

	return None, ''
