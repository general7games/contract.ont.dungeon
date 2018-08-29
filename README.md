SmartContract for dungeon, an ontology game.

* Setup development environment

	1. Windows (pre-built compiler)
	
		* Add bin/neon to your PATH
		* Install NeoContractPlugin in Visual Studio 2017

	2. Other platforms

		* Build neo-compiler
			1. git clone https://github.com/neo-project/neo-compiler in an separated directory
			2. git clone -b ont-dev https://github.com/xris-hu/neo-devpack-dotnet.git in another directory
			3. open neo-compiler/neo-compiler.sln
				* remove reference of NeoVM.dll from dependence
				* add dependence to neo-dev-pack-donet/net40/NeoVM.dll
				* change bCompatible variable in neon-compiler/neon/Program.cs to true by default
				* publish neon project

		* Build neo-compiler from repo https://github.com/general7games/neo-compiler.git

			This repo did all steps above

	3. Important, use --compatible to compile contracts

* Setup deployment environment

	*Steps below maybe different from other platforms*

	1. Install Python2 and setup a VirtualEnv 
		```
		C:\Python27\Python -m virtualenv _PYENV
		```

	2.  Install requirements
		``
		_PYENV\Scripts\pip.exe install -r pyscript\requirements.txt
		```
	
	3. Copy deploy.json.template to deploy.json and fill your account information.

		* if you dont have any account, check *Create deployment account* first.

* Create deployment account

	use *_PYENV\Scripts\python.exe bin\pyscript\CreateAccount<span></span>.py* to create deploy account.
	
	Parameters of CreateDeployAccount,

		nshost, backend address

	**PLEASE BACK UP YOUR ACCOUNT AND PASSWORD IN A SAFE PLACE**

