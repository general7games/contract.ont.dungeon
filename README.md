SmartContract for dungeon, an ontology game.

* Setup development environment

	1. Windows (pre-built compiler)
	
		* Add bin/neon to your PATH
		* Install NeoContractPlugin in Visual Studio 2017

	2. Other platforms

		* Build Neo compiler
			1. git clone https://github.com/neo-project/neo-compiler in an separated directory
			2. git clone -b ont-dev https://github.com/xris-hu/neo-devpack-dotnet.git in another directory
			3. open neo-compiler/neo-compiler.sln
				* remove reference of NeoVM.dll from dependence
				* add dependence to neo-dev-pack-donet/net40/NeoVM.dll
				* publish neon project

	3. Important, use --compatible to compile contracts

 