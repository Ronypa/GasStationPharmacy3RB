INSERT INTO [dbo].[COMPAÑIA] ([Nombre],[Sede],[Activo])
     VALUES('Farmacias Phischel', 'USA', 1)
GO

INSERT INTO [dbo].[COMPAÑIA] ([Nombre],[Sede],[Activo])
     VALUES('BombaTica', 'USA', 1)
GO


INSERT INTO [dbo].[SUCURSAL] ([Nombre], [Provincia], [Cuidad], [Señas]
           ,[Descripcion], [Compañia], [Activo])
     VALUES ('Phischel Central','Cartago','Centro','Contiguo a la Municipalidad'
           ,'Sede central','Farmacias Phischel',1)
GO

INSERT INTO [dbo].[SUCURSAL] ([Nombre], [Provincia], [Cuidad], [Señas]
           ,[Descripcion], [Compañia], [Activo])
     VALUES ('Phischel PZ','San Jose','Perez Zeledon','Costado norte de la escuela doce de Marzo'
           ,'Sede PZ','Farmacias Phischel',1)
GO

INSERT INTO [dbo].[SUCURSAL] ([Nombre], [Provincia], [Cuidad], [Señas]
           ,[Descripcion], [Compañia], [Activo])
     VALUES ('BombaTica Alajuela','Alajuela','Poas','Costado sur del estadio Alejandro Morera Soto'
           ,'BombaTica LDA','BombaTica',1)
GO

INSERT INTO [dbo].[SUCURSAL] ([Nombre], [Provincia], [Cuidad], [Señas]
           ,[Descripcion], [Compañia], [Activo])
     VALUES ('BombaTica Nicoya','Guanacaste','Nicoya','Contiguo al estadio Edgardo Baltodano'
           ,'Sede Nicoya','BombaTica',1)
GO

INSERT INTO [dbo].[ROL]([Nombre],[Descripcion],[Activo])
     VALUES('Administrador','Administrador acceso total',1)
GO

INSERT INTO [dbo].[ROL]([Nombre],[Descripcion],[Activo])
     VALUES('Farmaceutico','Farmaceutico con acceso en sucursales',1)
GO

INSERT INTO [dbo].[ROL]([Nombre],[Descripcion],[Activo])
     VALUES('Doctor','Atencion a los clientes',1)
GO

INSERT INTO [dbo].[ROL]([Nombre],[Descripcion],[Activo])
     VALUES('Cajero','Control de dinero',1)
GO

INSERT INTO [dbo].[EMPLEADO]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia],
	[Cuidad],[Señas],[FechaNacimiento],[Contraseña],[Sucursal],[Activo],[Rol])
     VALUES (303150415,'Pedro','Alberto','Perez','Lopez','Cartago','Tejar'
           ,'100 metros sur de Restaurante El Quijongo','15-02-1980'
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','1234'),'Phischel Central',1,'Administrador')
GO

INSERT INTO [dbo].[EMPLEADO]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia],
	[Cuidad],[Señas],[FechaNacimiento],[Contraseña],[Sucursal],[Activo],[Rol])
     VALUES (108490249,'Jose','Julian','Arias','Mora','San Jose','San Pedro'
           ,'Contiguo UCR','01-10-1970'
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','12345'),'BombaTica Alajuela',1,'Administrador')
GO

INSERT INTO [dbo].[EMPLEADO]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia],
	[Cuidad],[Señas],[FechaNacimiento],[Contraseña],[Sucursal],[Activo],[Rol])
     VALUES (205911452,'Marco','Luis','Quesada','Rojas','Heredia','Belen'
           ,'La Rivera de Belen','10-01-1990'
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','123456'),'Phischel Central',1,'Farmaceutico')
GO

INSERT INTO [dbo].[EMPLEADO]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia],
	[Cuidad],[Señas],[FechaNacimiento],[Contraseña],[Sucursal],[Activo],[Rol])
     VALUES (405670923,'Ronny','Fabian','Romero','Nuñez','Limon','Sixaola'
           ,'contiguo a la iglesia sixaola','23-06-1987'
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','1234567'),'Phischel PZ',1,'Farmaceutico')
GO

INSERT INTO [dbo].[EMPLEADO]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia],
	[Cuidad],[Señas],[FechaNacimiento],[Contraseña],[Sucursal],[Activo],[Rol])
     VALUES (505821723,'Olman','Dennis','Fajardo','Calvo','Alajuela','Poas'
           ,'carretera al volcan Poas','19-08-1973'
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','a12d'),'BombaTica Nicoya',1,'Farmaceutico')
GO

INSERT INTO [dbo].[EMPLEADO]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia],
	[Cuidad],[Señas],[FechaNacimiento],[Contraseña],[Sucursal],[Activo],[Rol])
     VALUES (614580522,'Allen','Saul','McDonald','Brenes','Puntarenas','Sardinal'
           ,'ruta a Caldera','04-07-1992'
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','9gfa'),'BombaTica Alajuela',1,'Farmaceutico')
GO

INSERT INTO [dbo].[CASAFARMACEUTICA]([Nombre],[Sede],[Activo])
     VALUES('Bayer','USA',1)
GO

INSERT INTO [dbo].[CASAFARMACEUTICA]([Nombre],[Sede],[Activo])
     VALUES('Novaris','CR',1)
GO

INSERT INTO [dbo].[CASAFARMACEUTICA]([Nombre],[Sede],[Activo])
     VALUES('Roche','USA',1)
GO

INSERT INTO [dbo].[CASAFARMACEUTICA]([Nombre],[Sede],[Activo])
     VALUES('Pfizer','Mexico',1)
GO

INSERT INTO [dbo].[CASAFARMACEUTICA]([Nombre],[Sede],[Activo])
     VALUES('Merck','USA',1)
GO

INSERT INTO [dbo].[CASAFARMACEUTICA]([Nombre],[Sede],[Activo])
     VALUES('SANOFI','CR',1)
GO


INSERT INTO [dbo].[ADMINISTRADORXSUCURSAL]([Administrador],[Sucursal],[Activo])
     VALUES(108490249,'BombaTica Nicoya',1)
GO

INSERT INTO [dbo].[ADMINISTRADORXSUCURSAL]([Administrador],[Sucursal],[Activo])
     VALUES(108490249,'BombaTica Alajuela',1)
GO

INSERT INTO [dbo].[ADMINISTRADORXSUCURSAL]([Administrador],[Sucursal],[Activo])
     VALUES(303150415,'Phischel Central',1)
GO

INSERT INTO [dbo].[ADMINISTRADORXSUCURSAL]([Administrador],[Sucursal],[Activo])
     VALUES(303150415,'Phischel PZ',1)
GO

INSERT INTO [dbo].[MEDICAMENTO]([Nombre],[Prescripcion],[Cantidad],[CasaFarmaceutica]
           ,[Activo],[Precio])
     VALUES ('Panadol ExtraFuerte',0,1000,'Bayer',1,200)
GO

INSERT INTO [dbo].[MEDICAMENTO]([Nombre],[Prescripcion],[Cantidad],[CasaFarmaceutica]
           ,[Activo],[Precio])
     VALUES ('Gex Noche',0,300,'SANOFI',1,175)
GO

INSERT INTO [dbo].[MEDICAMENTO]([Nombre],[Prescripcion],[Cantidad],[CasaFarmaceutica]
           ,[Activo],[Precio])
     VALUES ('Gex Día',0,500,'Pfizer',1,225)
GO

INSERT INTO [dbo].[MEDICAMENTO]([Nombre],[Prescripcion],[Cantidad],[CasaFarmaceutica]
           ,[Activo],[Precio])
     VALUES ('Inyección Insulina',0,100,'SANOFI',1,12000)
GO

INSERT INTO [dbo].[MEDICAMENTO]([Nombre],[Prescripcion],[Cantidad],[CasaFarmaceutica]
           ,[Activo],[Precio])
     VALUES ('Alka Seltzer',0,1200,'Roche',1,75)
GO

INSERT INTO [dbo].[MEDICAMENTO]([Nombre],[Prescripcion],[Cantidad],[CasaFarmaceutica]
           ,[Activo],[Precio])
     VALUES ('Panadol ULTRA',0,1500,'Novaris',1,350)
GO


INSERT INTO [dbo].[CLIENTE]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia]
           ,[Cuidad],[Senas],[FechaNacimiento],[Prioridad],[Contrasena],[Activo])
     VALUES (305430785,'Raul','De los Angeles','Arias','Quesada','Cartago','Bermejo'
           ,'150 metros norte de la escuela de Bermejo','02-06-1996',1
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','rolo'),1)
GO

INSERT INTO [dbo].[CLIENTE]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia]
           ,[Cuidad],[Senas],[FechaNacimiento],[Prioridad],[Contrasena],[Activo])
     VALUES (509450161,'Rony','Jose','Paniagua','Colindres','Guanacaste','Nandayure'
           ,'500 metros sur de la pulperia Don Paco','03-09-1996',1
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','pani'),1)
GO

INSERT INTO [dbo].[CLIENTE]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia]
           ,[Cuidad],[Senas],[FechaNacimiento],[Prioridad],[Contrasena],[Activo])
     VALUES (116220539,'Bryan','Stephen','Abarca','Huever','San Jose','Perez Zeledon'
           ,'contiguo al bar Mis Cositas','07-11-1997',1
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','huever'),1)
GO

INSERT INTO [dbo].[CLIENTE]([Cedula],[Nombre1],[Nombre2],[Apellido1],[Apellido2],[Provincia]
           ,[Cuidad],[Senas],[FechaNacimiento],[Prioridad],[Contrasena],[Activo])
     VALUES (306380922,'Ronny','Nicky','Quesada','Arias','Cartago','Coris'
           ,'350 metros este de la escuela de Coris','23-09-1996',1
		   ,ENCRYPTBYPASSPHRASE ('GaStFa3RB','nicky'),1)
GO


INSERT INTO [dbo].[TELEFONOXCLIENTE]([Cliente],[Telefono],[Activo])
     VALUES(509450161,80564738,1)
GO

INSERT INTO [dbo].[TELEFONOXCLIENTE]([Cliente],[Telefono],[Activo])
     VALUES(509450161,25730904,1)
GO

INSERT INTO [dbo].[TELEFONOXCLIENTE]([Cliente],[Telefono],[Activo])
     VALUES(306380922,70837343,1)
GO

INSERT INTO [dbo].[TELEFONOXCLIENTE]([Cliente],[Telefono],[Activo])
     VALUES(306380922,22478490,1)
GO

INSERT INTO [dbo].[TELEFONOXCLIENTE]([Cliente],[Telefono],[Activo])
     VALUES(305430785,70537424,1)
GO

INSERT INTO [dbo].[TELEFONOXCLIENTE]([Cliente],[Telefono],[Activo])
     VALUES(116220539,60363619,1)
GO


INSERT INTO [dbo].[PADECIMIENTO]([Cliente],[Padecimiento],[Año],[Activo])
     VALUES(509450161,'bronquitis',2000,1)
GO

INSERT INTO [dbo].[PADECIMIENTO]([Cliente],[Padecimiento],[Año],[Activo])
     VALUES(306380922,'Diabetes',2005,1)
GO

INSERT INTO [dbo].[PADECIMIENTO]([Cliente],[Padecimiento],[Año],[Activo])
     VALUES(509450161,'asma',1998,1)
GO

INSERT INTO [dbo].[PADECIMIENTO]([Cliente],[Padecimiento],[Año],[Activo])
     VALUES(116220539,'conjunivitis',2010,1)
GO