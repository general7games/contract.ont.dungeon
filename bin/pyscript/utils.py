import md5

def get_url(config, path):
	if not config.has_key('host'):
		return 'missing config.host', None
	host = str(config['host'])
	if not host.endswith('/'):
		host = host + '/'
	if path.startswith('/'):
		path = path[1:]
	return None, host + path

def enc_password(password):
	m = md5.new()
	m.update(password)
	return m.hexdigest()
