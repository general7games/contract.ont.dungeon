import argparse
import json
import sys
import os
import datetime
import shutil

import account
import contract

def backup_config(filename):
	n = os.path.splitext(filename)
	date = datetime.datetime.utcnow().isoformat().replace(':', '_')
	shutil.copy(filename, n[0] + '.' + date + n[1])

def write_config(config, filename):
	with open(filename, 'w') as f:
		json.dump(config, f, indent=4)

if __name__ == '__main__':
	parser = argparse.ArgumentParser()
	parser.add_argument('--config', help='specific witch config to use', required=True)
	parser.add_argument('--password')
	parser.add_argument('--avm', help='Compiled contract file path (.avm)')
	parser.add_argument('--author', help='Required when deploy. It overrides value in config. And it updates author of contract when migrate.')
	parser.add_argument('--email', help='Required when deploy. It overrides value in config. And it updates email of contract when migrate')
	parser.add_argument('--storage', action='store_true', help='required when deploy.')
	parser.add_argument('--description', help='Required when deploy. It updates description of contract when migrate.')
	parser.add_argument('command', nargs=1, help='init | deploy | migrate')
	args = parser.parse_args()

	config = None
	password = None

	with open(args.config) as f:
		config = json.load(f)
	if not config.has_key(password) and not args.password:
		print >> sys.stderr, 'should provide password from config or commandline options'
		sys.exit(1)

	if args.password:
		password = args.password
	else:
		password = config.password

	if args.command[0] == 'init':
		(err, address) = account.init_admin(config, password)
		if err:
			print >> sys.stderr, err
			sys.exit(1)
		if config.has_key('address'):
			print >> sys.stdout, 'WARING! overwrite address in ' + args.config
			backup_config(args.config)
		config['address'] = address
		write_config(config, args.config)

	elif args.command[0] == 'deploy':
		if not args.avm:
			print >> sys.stderr, 'missing --avm'
			parser.print_help()
			sys.exit(1)

		author = config.get('author', args.author)
		if not author:
			print >> sys.stderr, 'missing --author'
			parser.print_help()
			sys.exit(1)

		email = config.get('email', args.email)
		if not email:
			print >> sys.stderr, 'missing --email'
			parser.print_help()
			sys.exit(1)

		if not args.description:
			print >> sys.stderr, 'missing --description'
			parser.print_help()
			sys.exit(1)

		(err, result) = contract.deploy(config, args.avm, password, args.storage, author, email, args.description)
		if err:
			print >> sys.stderr, err
			sys.exit(1)
		print >> sys.stdout, result
