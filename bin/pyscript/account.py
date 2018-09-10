import requests
import md5
import sys
import md5

import utils

def init_admin(config, password):
	(err, url) = utils.get_url(config, '/admin/init')
	if err:
		return err, None
	encPassword = utils.enc_password(password)
	res = requests.post(url, json={ 'password': encPassword })
	content = res.json()
	if content['error'] != 0:
		if content['error'] == 10400:
			return 'admin already initialized', None
		return 'error ' + str(content['error'])
	return None, content['result']['address']
