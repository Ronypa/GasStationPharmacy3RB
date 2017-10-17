//Controlador para el login tiene la funcion que se llama desde el html (hace uso del servicio "loginService")

var ip = "http://192.168.100.2";

app.controller('logincontroller', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {

    $scope.abrirMenu = function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    };

    $scope.loginAdministrador = function () {
        var empleado =
            "grant_type=password" +
            "&userName=" + encodeURIComponent($scope.loginCedula) +
            "&password=" + encodeURIComponent($scope.loginPassword) +
            "&client_id=empleado"
            ;

        var apiRoute = ip + '/WebApi/token';
        var respuesta = ServicioHTTP.postLog(apiRoute, empleado);
        respuesta.then(function (response) {
            var accessToken = response.data.access_token;
            var compania = response.data.compañia;
            var sucursal = response.data.sucursal;
            authFact.setAccessToken(accessToken);
            authFact.setCompania(compania);
            authFact.setSucursal(sucursal);
            if (response.data.admin == "true") {
                $location.path("/admin");
            }

            else $location.path("/sucursal");
        },
            function (error) {
                authFact.setAccessToken("");
                alert("Usuario y/o contraseña incorrecto");
            }
        );
    }

    $scope.loginCliente = function () {
        var cliente =
            "userName=" + encodeURIComponent($scope.loginCedula) +
            "&password=" + encodeURIComponent($scope.loginPassword) +
            "&grant_type=password" +
            "&client_id=cliente"
            ;

        var apiRoute = ip + '/WebApi/token';
        var respuesta = ServicioHTTP.postLog(apiRoute, cliente);
        respuesta.then(function (response) {

            var accessToken = response.data.access_token;
            authFact.setAccessToken(accessToken);
            $location.path("/clientesPedidos");
        },
            function (error) {
                authFact.setAccessToken("");
                alert("Usuario y/o contraseña incorrecto");
            }
        );
    };

    //funciones para redireccionar a las vistas de log in cuendo se activa algun boton
    $scope.loginadministrador = function () {
        $location.path("/loginadministrador");
    };
    $scope.logincliente = function () {
        $location.path("/logincliente");
    };
    $scope.registrocliente = function () {
        $location.path("/registrocliente");
    };
});


/*Controlador para la administracion de los clientes, aqui estan las funciones de editar, eliminar y crear clientes*/
app.controller('adminCliController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html"; //para poner el menu lateral en la vista de administracion de clientes
    $scope.GetAllClientes = function () {
        var apiRoute = ip + '/WebApi/consultarClientes';
        var clientes = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        clientes.then(function (response) {
            $scope.clientes = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarCliente = function (borrar) {
        if (confirm("Seguro que desea borrar el registro?")) {
            var apiRoute = ip + '/WebApi/borrarClientes';
            var clientes = ServicioHTTP.postToken(apiRoute, borrar, authFact.getAccessToken());
            clientes.then(function (response) {
                $scope.GetAllClientes();
            },
                function (error) {
                    alert("Error al borrar el cliente");
                });
        }
    };


    /*variable para cargar todos los clientes */
    $scope.editar = function (cliente) {
        $scope.cliente = angular.copy(cliente);
    };
    
    /*funcion que se llama para cargar los cambios realizados*/
    $scope.aplicarCambios = function () {
        var cliente =
            {
                nombre1: $scope.cliente.nombre1,
                nombre2: $scope.cliente.nombre2,
                apellido1: $scope.cliente.apellido1,
                apellido2: $scope.cliente.apellido2,
                provincia: $scope.provi,
                ciudad: $scope.cliente.ciudad,
                señas: $scope.cliente.senas,
                fechaNacimiento: $scope.cliente.fechaNacimiento,
                cedula: $scope.cliente.cedula
            };
        var apiRoute = ip + '/WebApi/actualizarClienteAdmin';
        var respuesta = ServicioHTTP.postToken(apiRoute, cliente, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Cliente actualizado.');
            document.getElementById('botoncerrar').click();
            $scope.GetAllClientes();
        },
            function (error) {
                alert('Error al actualizar el cliente.')
            }
        );
    };

    $scope.agregarCliente = function () {
        var cliente =
            {

                cedula: $scope.cedula,
                nombre1: $scope.nombre1,
                nombre2: $scope.nombre2,
                apellido1: $scope.apellido1,
                apellido2: $scope.apellido2,
                provincia: provincia,
                ciudad: $scope.ciudad,
                senas: $scope.senas,
                padecimientos: $scope.padecimientos,
                telefono: $.map($('input[name="telefono"]'), function (c) { return c.value; }),
                fechaNacimiento: $scope.fechaNacimiento
            };
        var apiRoute = ip + '/WebApi/agregarCliente';
        var respuesta = ServicioHTTP.postToken(apiRoute, cliente, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Cliente creado.');
            document.getElementById('botoncerrar2').click();
            $scope.GetAllClientes();
        },
            function (error) {
                alert('Error al crear el cliente.')
            }
        );
    };
    
    $scope.GetAllClientes();
});

/*Controlador para la administracion de los empleados, posee las funciones de eliminar, crear y editar*/
app.controller('adminEmpController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html"; //poner el menu lateral en la vista de administracion de empleados
    $scope.GetAllEmpleados = function () {

        var compania = { opcion: authFact.getCompania() }

        var apiRoute = ip + '/WebApi/consultarEmpleados';
        var clientes = ServicioHTTP.postToken(apiRoute, compania, authFact.getAccessToken());
        clientes.then(function (response) {
            $scope.empleados = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.GetAllSucursales = function () {
        var compania = {
            opcion: authFact.getCompania()
        };
        var apiRoute = ip + '/WebApi/consultarSucursalesC';
        var sucursal = ServicioHTTP.postToken(apiRoute, compania, authFact.getAccessToken());
        sucursal.then(function (response) {
            $scope.sucursales = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.GetAllRoles = function () {
        var apiRoute = ip + '/WebApi/consultarRolesC';
        var rol = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        rol.then(function (response) {
            $scope.roles = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarEmpleado = function (borrar) {
        if (confirm("Seguro que desea borrar el registro?")) {
            var apiRoute = ip + '/WebApi/borrarEmpleados';
            var empleados = ServicioHTTP.postToken(apiRoute, borrar, authFact.getAccessToken());
            empleados.then(function (response) {
                $scope.GetAllEmpleados();
            },
                function (error) {
                    alert("Error eliminando el empleado, el empleado es administrador");
                });
        }
    };
    /*funcion que se llama para cargar los cambios realizados*/
    $scope.aplicarCambios = function () {
        var empleado =
            {
                nombre1: $scope.empleado.nombre1,
                nombre2: $scope.empleado.nombre2,
                apellido1: $scope.empleado.apellido1,
                apellido2: $scope.empleado.apellido2,
                provincia: $scope.empleado.provincia,
                ciudad: $scope.empleado.ciudad,
                senas: $scope.empleado.senas,
                fechaNacimiento: $scope.empleado.fechaNacimiento,
                cedula: $scope.empleado.cedula,
                 rol: $scope.empleado.rol,
                sucursal: $scope.empleado.sucursal
            };
        var apiRoute = ip + '/WebApi/actualizarEmpleado';
        var empleado = ServicioHTTP.postToken(apiRoute, empleado, authFact.getAccessToken());
        empleado.then(function (response) {
            document.getElementById('botoncerrar').click();
            alert('Empleado actualizado.');
            $scope.GetAllEmpleados();
        },
            function (error) {
                alert("Error al actualiar el empleado");
            });
    };




    $scope.agregarEmpleado = function () {
        var empleado =
            {
                nombre1: $scope.nombre1,
                nombre2: $scope.nombre2,
                apellido1: $scope.apellido1,
                apellido2: $scope.apellido2,
                provincia: $scope.provincia,
                ciudad: $scope.ciudad,
                senas: $scope.senas,
                fechaNacimiento: $scope.fechaNacimiento,
                cedula: $scope.cedula,
                contrasena: $scope.password,
                rol: $scope.rol,
                sucursal: $scope.sucursal
            };
        var apiRoute = ip + '/WebApi/agregarEmpleado';
        var empleado = ServicioHTTP.postToken(apiRoute, empleado, authFact.getAccessToken());
        empleado.then(function (response) {
            document.getElementById('botoncerrar2').click();
            alert('Empleado creado.');
            $scope.GetAllEmpleados();
        },
            function (error) {
                alert("Error al crear el empleado");
            });
    };
    /*variable para cargar todos los empleados */
    $scope.editar = function (empleado) {
        $scope.empleado = angular.copy(empleado);
   

    };

    $scope.GetAllSucursales();
    setTimeout(() => { $scope.GetAllRoles(); }, 500);
    setTimeout(() => { $scope.GetAllEmpleados(); }, 1000);
    
    
});

/*controlador para administrar los productos*/
app.controller('adminProduController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";//para poner el menu lateral en la vista de adminitracion de prodcutos
    $scope.GetAllProductos = function () {
        var apiRoute = ip + '/WebApi/consultarMedicamentos';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.productos = response.data;
            $scope.interpretarPrescripcion($scope.productos);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };


    $scope.GetAllCasaFarmaceutica = function () {
        var apiRoute = ip + '/WebApi/consultarCasaFarmaceuticaC';
        var casaFarmaceutica = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        casaFarmaceutica.then(function (response) {
            $scope.casaFarmaceuticas = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };



    $scope.BorrarProducto = function (borrar) {
        if (confirm("¿Seguro que desea borrar el producto?")) {
            var apiRoute = ip + '/WebApi/borrarMedicamento';
            var obj = {
                opcion: borrar
            };
            var respuesta = ServicioHTTP.postToken(apiRoute, obj, authFact.getAccessToken());

            respuesta.then(function (response) {
                $scope.GetAllProductos();
            },
                function (error) {
                    alert("Error.");
                });
        }
    };
    /*funcion que se llama para cargar los cambios realizados*/
    $scope.aplicarCambios = function () {
        var producto =
            {
                nombre: $scope.producto.nombre,
                cantidad: $scope.producto.cantidad,
                prescripcion: $scope.producto.prescripcion,
                casaFarmaceutica: $scope.producto.casaFarmaceutica,
                precio: $scope.producto.precio

            };
        var apiRoute = ip + '/WebApi/actualizarMedicamento';
        var respuesta = ServicioHTTP.postToken(apiRoute, producto, authFact.getAccessToken());
        $scope.interpretarPrescripcion([producto])
        respuesta.then(function (response) {
            alert('Producto actualizado');
            $scope.GetAllProductos();
            document.getElementById('botoncerrar').click();
        },
            function (error) {
                alert('Error al actualizar el producto');
            });
    };

    $scope.agregarProducto = function () {
        var producto =
            {
                nombre: $scope.nombre,
                cantidad: $scope.cantidad,
                prescripcion: $scope.producto.prescripcion,
                casaFarmaceutica: $scope.casaFarmaceutica,
                precio: $scope.precio
            };
        $scope.interpretarPrescripcion([producto]); //hay que probar esto porque ni puta idea si sirve poner esos parentesis asi
        var apiRoute = ip + '/WebApi/agregarMedicamento';
        var respuesta = ServicioHTTP.post(apiRoute, producto, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Producto creado');
            $scope.GetAllProductos();
            document.getElementById('botoncerrar2').click();
        },
            function (error) {
                alert('Error al crear el producto');
            });
    };
    /*variable para cargar todos los productos */
    $scope.editar = function (producto) {
        $scope.producto = angular.copy(producto);
    };
    /*Esta funcion se utiliza para pasar un 'true' a un Si y un 'false' a un no y viceversa*/
    $scope.interpretarPrescripcion = function (productos) {
        for (var index = 0; index < productos.length; index++) {
            if (productos[index].prescripcion === true)
                productos[index].prescripcion = 'Sí';
            else if (productos[index].prescripcion === false)
                productos[index].prescripcion = 'No';
            else if (productos[index].prescripcion === 'Sí')
                productos[index].prescripcion = true;
            else
                productos[index].prescripcion = false;
        }
    };

    $scope.GetAllProductos();
    $scope.GetAllCasaFarmaceutica();
});

/*controlador para administrar los roles*/
app.controller('adminRolController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";//para poner el menu lateral en la vista administracion de roles
    $scope.GetAllRoles = function () {
        var apiRoute = ip + '/WebApi/consultarRoles';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.roles = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };




    $scope.BorrarRol = function (borrar) {
        var obj = {
            opcion: borrar
        };
        if (confirm("¿Seguro que desea borrar el rol?")) {
            var apiRoute = ip + '/WebApi/borrarRol';
            var respuesta = ServicioHTTP.postToken(apiRoute, obj, authFact.getAccessToken());
            respuesta.then(function (response) {
                $scope.GetAllRoles();
            },
                function (error) {
                    alert("Error cargando datos");
                });
        }
    };
    /*funcion que se llama para aplicar los cambios realizados en la acualizacion del rol*/
    $scope.aplicarCambios = function () {
        var rol =
            {
                nombre: $scope.rol.nombre,
                descripcion: $scope.rol.descripcion
            };
        var apiRoute = ip + '/WebApi/actualizarRol';
        var respuesta = ServicioHTTP.postToken(apiRoute, rol, authFact.getAccessToken());
        respuesta.then(function (response) {
            document.getElementById('botoncerrar').click();
            alert('Rol actualizado.');
            $scope.GetAllRoles();
            
        },
            function (error) {
                alert('Error al actualizar el rol.')
            }
        );
    };

    $scope.agregarRol = function () {
        var rol =
            {
                nombre: $scope.nombre,
                descripcion: $scope.descripcion
            };
        var apiRoute = ip + '/WebApi/agregarRol';
        var respuesta = ServicioHTTP.postToken(apiRoute, rol, authFact.getAccessToken());
        respuesta.then(function (response) {
            document.getElementById('botoncerrar2').click();
            alert('Rol creado.');
            $scope.GetAllRoles();  
        },
            function (error) {
                alert('Error al crear el rol.')
            }
        );
    };
    /*variable para cargar todos los clientes */
    $scope.editar = function (rol) {
        $scope.rol = angular.copy(rol);
    };

    $scope.GetAllRoles();
});

/*controlador para administrar las sucursales*/
app.controller('adminSucuController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";//para poner el menu lateral en la vista de admin de sucursales
    $scope.GetAllSucursales = function () {
        var apiRoute = ip + '/WebApi/consultarSucursales';
        var compania = {
            opcion: authFact.getCompania() 

        };
        var respuesta = ServicioHTTP.postToken(apiRoute, compania, authFact.getAccessToken());
        respuesta.then(function (response) {

            $scope.sucursales = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarSucursal = function (borrar) {
        var obj = {
            opcion: borrar
        };
        if (confirm("¿Seguro que desea borrar la sucursal?")) {
            var apiRoute = ip + '/WebApi/borrarSucursal';
            var respuesta = ServicioHTTP.postToken(apiRoute, obj, authFact.getAccessToken());
            respuesta.then(function (response) {
                $scope.GetAllSucursales();
            },
                function (error) {
                    alert("Error eliminando sucursal, la sucursal posee empleado o pedidos activos");
                });
        }
    };

    /*se llama para aplicarle los cambios realizados a una sucursal*/
    $scope.aplicarCambios = function () {
        var sucursal =
            {
                nombre: $scope.sucursal.nombre,
                provincia: $scope.sucursal.provincia,
                ciudad: $scope.sucursal.ciudad,
                senas: $scope.sucursal.senas,
                descripcion: $scope.sucursal.descripcion,
                administrador: $scope.sucursal.administrador
                
       
            };
        var apiRoute = ip + '/WebApi/actualizarSucursal';
        var respuesta = ServicioHTTP.postToken(apiRoute, sucursal, authFact.getAccessToken());
        respuesta.then(function (response) {
            document.getElementById("botoncerrar").click();
            alert('Sucursal actualizada.');
            $scope.GetAllSucursales();
        },
            function (error) {
                alert('Error al actualizar.');
            }
        );
    };

    $scope.agregarSucursal = function () {
        var sucursal =
            {
                nombre: $scope.nombre,
                provincia: $scope.provincia,
                ciudad: $scope.ciudad,
                senas: $scope.senas,
                descripcion: $scope.descripcion,
                compania: authFact.getCompania(),
                administrador: $scope.cedula
            };
        var apiRoute = ip + '/WebApi/agregarSucursal';
        var respuesta = ServicioHTTP.postToken(apiRoute, sucursal, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Sucursal creada.');
            document.getElementById("botoncerrar2").click();
            $scope.GetAllSucursales();
        },
            function (error) {
                alert('Error al crear.');
            }
        );
    };

   /*variable para cargar todas las sucursales*/
    $scope.editar = function (sucursal) {
        $scope.sucursal = angular.copy(sucursal);
    };


    $scope.GetAllSucursales();
});

/*controlador para administrar las estadisticas*/
app.controller('adminEstaController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";//para poner el menu lateral en la vista de admin estadisticas
    
    var valores1 = [];
    var etiquetas1 = [];

    var valores2 = [];
    var etiquetas2 = [];

    var valores3 = [];
    var etiquetas3 = [];

    $scope.cargarProductosTotal = function () {
        var apiRoute = ip + '/WebApi/estadisticaProductosTotal';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            angular.forEach(response.data, function (value, key) {
                valores2.push(value.cantidad);
                etiquetas2.push(value.opcion);
            });
        },
            function (error) {
                alert('Error al actualizar.');
            }
        );
    };

    $scope.cargarProductosCompania = function () {
        var postCompania =
            {
                opcion: authFact.getCompania()
            };
        var apiRoute = ip + '/WebApi/estadisticaProductosCompania';
        var respuesta = ServicioHTTP.postToken(apiRoute, postCompania, authFact.getAccessToken());
        respuesta.then(function (response) {
            angular.forEach(response.data, function (value, key) {
                valores1.push(value.cantidad);
                etiquetas1.push(value.opcion);
            });
        },
            function (error) {
                alert('Error al actualizar.');
            }
        );
    };

    $scope.cargarVentas = function () {
        var postCompania =
            {
                opcion: authFact.getCompania()
            };
        var apiRoute = ip + '/WebApi/estadisticaVentas';
        var respuesta = ServicioHTTP.postToken(apiRoute, postCompania, authFact.getAccessToken());
        respuesta.then(function (response) {
            valores3.push(response.data);
            etiquetas3.push("Ventas");
        },
            function (error) {
                alert('Error al actualizar.');
            }
        );
    };

    $scope.cargarProductosCompania();
    setTimeout(() => { $scope.cargarProductosTotal(); }, 500);
    setTimeout(() => { $scope.cargarVentas(); }, 1000);

    setTimeout(() => {
        var ctx = document.getElementById("grafico1").getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: etiquetas1,
                datasets: [{
                    label: 'cantidad',
                    data: valores1,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });

        var ctx2 = document.getElementById("grafico2").getContext('2d');
        var myChart2 = new Chart(ctx2, {
            type: 'bar',
            data: {
                labels: etiquetas2,
                datasets: [{
                    label: 'cantidad',
                    data: valores2,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });

        var ctx3 = document.getElementById("grafico3").getContext('2d');
        var myChart3 = new Chart(ctx3, {
            type: 'bar',
            data: {
                labels: etiquetas3,
                datasets: [{
                    label: 'cantidad',
                    data: valores3,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });

    }, 1500);

});

/*Controlador para el registro de los clientes*/
app.controller('registercontroller', function ($scope, ServicioHTTP, $location) {

    $scope.registrarCliente = function () { //obtiene todos los datos del formulario
        var cliente =
            {
                nombre1: $scope.registroNombre1,
                nombre2: $scope.registroNombre2,
                apellido1: $scope.registroApellido1,
                apellido2: $scope.registroApellido2,
                provincia: $scope.registroProvincia,
                ciudad: $scope.registroCiudad,
                senas: $scope.registroSenas,
                fechaNacimiento: $scope.registroFechaNacimiento,
                cedula: $scope.registroCedula,
                contrasena: $scope.registroPassword,
                padecimientos: $scope.recuperar('padecimientos'),
                telefonos: $scope.recuperar('telefonos')
            };
        var apiRoute = ip + '/WebApi/agregarCliente';
        var respuesta = ServicioHTTP.post(apiRoute, cliente);
        respuesta.then(function (response) {
            if (confirm("Resgistro exitoso. ¿Desea ingresar ahora mismo?") === true)
                $location.path('/logincliente');
        },
            function (error) {
                alert("Error al registrarse. La cédula ingresada podría ya existir.");
            }
        );
    };


    /*metodo para obtener todos los telefonos o todos los padecimientos, el atributo 'cual' define que obtener*/
    $scope.recuperar = function (cual) {
        var resultado;
        if (cual === "telefonos") {
            var telefono = document.getElementById('tels').firstElementChild;//nodos de bloque donde esta los nuevos inputs de telefonos
            resultado = "[" + $scope.registroTel;
            while (telefono !== null) {
                resultado += ", " + telefono.firstElementChild.firstElementChild.firstElementChild.firstElementChild.value;
                telefono = telefono.nextElementSibling;
            }
        }
        else if (cual === "padecimientos") {
            if ($scope.registroPadecimiento === undefined || $scope.registroPadecimientoFecha === undefined) {
                return "["+"]";
            }
            var padecimiento = document.getElementById('dinamico').firstElementChild;//nodos de bloque donde estan los nuevos inputs de padecimientos
            resultado = "";
            resultado += "[{padecimiento:\'" + $scope.registroPadecimiento + "\', año:" + $scope.registroPadecimientoFecha.slice(0, 4) + "}";
            while (padecimiento !== null) {
                resultado += ",{padecimiento:\'" + padecimiento.firstElementChild.firstElementChild.firstElementChild.value + "\', año:";
                resultado += padecimiento.firstElementChild.nextElementSibling.firstElementChild.firstElementChild.value.slice(0, 4) + "}";
                padecimiento = padecimiento.nextElementSibling;
            }
        }
        resultado += "]";
        return resultado;
    };
    /*metodo para darle formato a las fechas a 'tipo dd/mm/aa', el parametro valor viene en formato 'aa-mm-d'*/
    $scope.convertirFecha = function (valor) {
        var fecha = new Date(valor);
        return ((fecha.getDate() + 1) + "/" + (fecha.getMonth() + 1) + "/" + fecha.getFullYear());
    };
});

/*Controlador para la pagina principal de Sucursal (Pedidos Nuevos)*/
app.controller('sucursalController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuSucursal = 'AngularJS/Templates/menuSucursal.html';
    $scope.GetAllPedidosNuevos = function () {
        var sucursal = {
            opcion: authFact.getSucursal()
        }

        var apiRoute = ip + '/WebApi/consultarPedidosNuevos';
        var respuesta = ServicioHTTP.postToken(apiRoute, sucursal, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.pedidosNuev = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    //funcion para pasar un pedido nuevo a pedido preparado
    $scope.preparado = function (Numpedido) {
        if (confirm("¿Seguro que desea establecer este pedido como preparado?")) {
            var apiRoute = ip + '/WebApi/pedidoPreparado';
            var respuesta = ServicioHTTP.postToken(apiRoute, Numpedido, authFact.getAccessToken());
            respuesta.then(function (response) {
                $scope.GetAllPedidosNuevos();
                alert("¡Pedido preparado!");

            },
                function (error) {
                    alert("ERROR");
                }
            );
        }
    };

    $scope.detalles = function (pedido) {
        $scope.pedido = pedido;
    };

    $scope.abrirMenu = function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    };

    $scope.GetAllPedidosNuevos();
});

/*Controlador para los Pedidos Preparados*/
app.controller('pedidosPreparadosController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuSucursal = 'AngularJS/Templates/menuSucursal.html';
    $scope.GetAllPedidosPrepa = function () {
        var sucursal = {
            opcion: authFact.getSucursal()
        }
        var apiRoute = ip + '/WebApi/consultarPedidosPreparados';
        var respuesta = ServicioHTTP.postToken(apiRoute, sucursal, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.pedidosPrepa = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };
    //funcion para pasar un pedido preparado a pedido facturado
    $scope.facturado = function (Numpedido) {
        if (confirm("¿Seguro que desea establecer este pedido como facturado?")) {
            var apiRoute = ip + '/WebApi/pedidoFacturado';
            var respuesta = ServicioHTTP.postToken(apiRoute, Numpedido, authFact.getAccessToken());
            respuesta.then(function (response) {
                $scope.GetAllPedidosPrepa();
                alert("¡Pedido facturado!");

            },
                function (error) {
                    alert("ERROR: No se pudo realizar lo solicitado.");
                }
            );
        }
    };

    $scope.detalles = function (pedido) {
        $scope.pedido = pedido;
    };

    $scope.abrirMenu = function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    };
    $scope.GetAllPedidosPrepa();
});

/*Controlador para los Pedidos Facturados*/
app.controller('pedidosFacturadosController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuSucursal = 'AngularJS/Templates/menuSucursal.html';
    $scope.GetAllPedidosFactu = function () {
        var sucursal = {
            opcion: authFact.getSucursal()
        }
        var apiRoute = ip + '/WebApi/consultarPedidosFacturados';
        var respuesta = ServicioHTTP.postToken(apiRoute, sucursal, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.pedidosFactu = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };
    //funcion para pasar un producto facturado como pedido retirado
    $scope.retirado = function (Numpedido) {
        if (confirm("¿Seguro que desea establecer este pedido como retirado?")) {
            var apiRoute = ip + '/WebApi/pedidoRetirado';
            var respuesta = ServicioHTTP.postToken(apiRoute, Numpedido, authFact.getAccessToken());
            respuesta.then(function (response) {
                $scope.GetAllPedidosFactu();
                alert("¡Pedido retirado!");

            },
                function (error) {
                    alert("ERROR: No se pudo realizar lo solicitado.");
                }
            );
        }
    };

    $scope.detalles = function (pedido) {
        $scope.pedido = pedido;
    };

    $scope.abrirMenu = function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    };
    $scope.GetAllPedidosFactu();
});

/*Controlador para los pedidos Retirados*/
app.controller('pedidosRetiradosController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuSucursal = 'AngularJS/Templates/menuSucursal.html';
    $scope.GetAllPedidosReti = function () {
        var sucursal = {
            opcion: authFact.getSucursal()
        }
        var apiRoute = ip + '/WebApi/consultarPedidosRetirados';
        var respuesta = ServicioHTTP.postToken(apiRoute, sucursal, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.pedidosReti = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.detalles = function (pedido) {
        $scope.pedido = pedido;
    };

    $scope.abrirMenu = function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    };
    $scope.GetAllPedidosReti();
});

