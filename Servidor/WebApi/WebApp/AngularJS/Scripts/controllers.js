//Controlador para el login tiene la funcion que se llama desde el html (hace uso del servicio "loginService")
app.controller('logincontroller', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {

    $scope.abrirMenu = function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
    };

    $scope.loginAdministrador = function () {
        var empleado =
            "userName=" + encodeURIComponent($scope.loginCedula) +
            "&password=" + encodeURIComponent($scope.loginPassword) +
            "&grant_type=password" +
            "&client_id=empleado"
            ;

        var apiRoute = '/WebApi/token';
        var respuesta = ServicioHTTP.post(apiRoute, empleado);
        respuesta.then(function (response) {

            var accessToken = response.data.access_token;
            authFact.setAccessToken(accessToken);
            $location.path("/admin");
        },
            function (error) {
                authFact.setAccessToken("");
                alert("Usuario y/o contraseña incorrecto");
            }
        );
    };

    $scope.loginCliente = function () {
        var cliente =
            "userName=" + encodeURIComponent($scope.loginCedula) +
            "&password=" + encodeURIComponent($scope.loginPassword) +
            "&grant_type=password" +
            "&client_id=cliente"
            ;

        var apiRoute = '/WebApi/token';
        var respuesta = ServicioHTTP.post(apiRoute, cliente);
        respuesta.then(function (response) {

            var accessToken = response.data.access_token;
            authFact.setAccessToken(accessToken);
            $location.path("/admin");
        },
            function (error) {
                authFact.setAccessToken("");
                alert("Usuario y/o contraseña incorrecto");
            }
        );
    };

    //funciones para redireccionar a las vistas de log in
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


app.controller('formController', function ($scope) {


   

    $scope.provinciaList = [
        { ProvinciaId: 1, Nombre: 'Cartago' },
        { ProvinciaId: 2, Nombre: 'San Jose' },
        { ProvinciaId: 3, Nombre: 'Puntaneras' },
        { ProvinciaId: 4, Nombre: 'Guanacaste' },
        { ProvinciaId: 5, Nombre: 'Limon' },
        { ProvinciaId: 6, Nombre: 'Alajuela' },
        { ProvinciaId: 7, Nombre: 'Heredia' }

    ];

    // function to submit the form after all validation has occurred			
    $scope.submitForm = function () {

        // Set the 'submitted' flag to true
        $scope.submitted = true;

        if ($scope.clienteForm.$valid) {
            alert("Formulario Valido!");
        }
        else {
            alert("Errores en el formulario!");
        }
    };
});


app.controller('adminCliController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";
    $scope.GetAllClientes = function () {
        var apiRoute = '/WebApi/consultarClientes';
        var clientes = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        clientes.then(function (response) {
            $scope.clientes = response.data;
            console.log(response.data);
        },
        function (error) {
            alert("Error cargando datos");
        });
    };

    $scope.editar = function (cliente) {
        $scope.cliente = cliente;
    };

    $scope.BorrarCliente = function (borrar) {
        console.log(borrar);
        if (confirm("Seguro que desea borrar el registro?")) {
            var apiRoute = '/WebApi/borrarClientes';
            var clientes = ServicioHTTP.post(apiRoute, borrar);
            clientes.then(function (response) {
                $scope.GetAllClientes();
                console.log(response.data);
            },
                function (error) {
                    alert("Error cargando datos");
                });
        }
    };
    $scope.GetAllClientes();
});


app.controller('adminEmpController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";
    $scope.GetAllEmpleados = function () {
        var apiRoute = '/WebApi/consultarEmpleados';
        var clientes = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        clientes.then(function (response) {
            $scope.empleados = response.data;
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarCliente = function (borrar) {
        console.log(borrar);
        if (confirm("Seguro que desea borrar el registro?")) {
            var apiRoute = '/WebApi/borrarEmpleados';
            var empleados = ServicioHTTP.post(apiRoute, borrar);
            empleados.then(function (response) {
                $scope.GetAllEmpleados();
                console.log(response.data);
            },
                function (error) {
                    alert("Error cargando datos");
                });
        }
    };

    $scope.GetAllEmpleados();
});

/*controlador para administrar los productos*/
app.controller('adminProduController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";
    $scope.GetAllProductos = function () {
        var apiRoute = '/WebApi/consultarMedicamentos';
        var respuesta = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        respuesta.then(function (response) {
            $scope.productos = response.data;
            //$scope.productos.prescripcion = 'Sí';
            if ($scope.productos.prescripcion === true)
                $scope.productos.prescripcion = 'Sí';
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarProducto = function (borrar) {
        if (confirm("¿Seguro que desea borrar el producto?")) {
            var apiRoute = '/WebApi/borrarMedicamento';
            var obj = {
                opcion: borrar
            };
            var respuesta = ServicioHTTP.post(apiRoute, obj);
            console.log(borrar);

            respuesta.then(function (response) {
                $scope.GetAllProductos();
                console.log(response.data);
            },
                function (error) {
                    alert("Error.");
                });
        }
    };
    $scope.GetAllProductos();
});

/*controlador para administrar los roles*/
app.controller('adminRolController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";
    $scope.GetAllRoles = function () {
        var apiRoute = '/WebApi/consultarRoles';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.roles = response.data;
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarRol = function (borrar) {
        console.log(borrar);
        if (confirm("¿Seguro que desea borrar el rol?")) {
            var apiRoute = '/WebApi/borrarProducto';
            var respuesta = ServicioHTTP.post(apiRoute, borrar);
            respuesta.then(function (response) {
                $scope.GetAllProductos();
                console.log(response.data);
            },
                function (error) {
                    alert("Error cargando datos");
                });
        }
    };
    $scope.GetAllRoles();
});

/*controlador para administrar las sucursales*/
app.controller('adminSucuController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";
    $scope.GetAllRoles = function () {
        var apiRoute = '/WebApi/consultarSucursales';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.sucursales = response.data;
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.BorrarSucursal = function (borrar) {
        console.log(borrar);
        if (confirm("¿Seguro que desea borrar la sucursal?")) {
            var apiRoute = '/WebApi/borrarProducto';
            var respuesta = ServicioHTTP.post(apiRoute, borrar);
            respuesta.then(function (response) {
                $scope.GetAllProductos();
                console.log(response.data);
            },
                function (error) {
                    alert("Error cargando datos");
                });
        }
    };
    $scope.GetAllRoles();
});

/*controlador para administrar las estadisticas*/
app.controller('adminEstaController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlAdmin = "AngularJS/Templates/menuLateral.html";
});

app.controller('registercontroller', function ($scope, ServicioHTTP, $location) {

    $scope.registrarCliente = function () {
        var cliente =
            {
                nombre1: $scope.registroNombre1,
                nombre2: $scope.registroNombre2,
                apellido1: $scope.registroApellido1,
                apellido2: $scope.registroApellido2,
                provincia: $scope.registroProvincia,
                ciudad: $scope.registroCiudad,
                señas: $scope.registroSenas,
                fechaNacimiento: $scope.convertirFecha($scope.registroFechaNacimiento),
                cedula: $scope.registroCedula,
                contrasena: $scope.registroPassword,
                //",registroPadecimiento:" + $scope.recuperar('padecimientos') +
                //",registroTel:" + $scope.recuperar('telefonos');
                activo: true
            };
        console.log(cliente);
        var apiRoute = '/WebApi/agregarCliente';
        var respuesta = ServicioHTTP.post(apiRoute, cliente);
        respuesta.then(function (response) {
            console.log(response.data);
            $location.path('/logincliente');
        },
            function (error) {
                alert("Usuario y/o contraseña incorrecto");
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
            var padecimiento = document.getElementById('dinamico').firstElementChild;//nodos de bloque donde estan los nuevos inputs de padecimientos
            resultado = "";
            resultado += "[[" + $scope.registroPadecimimeto + "," + $scope.convertirFecha($scope.registroPadecimientoFecha) + "]";
            while (padecimiento !== null) {
                resultado += ",[" + padecimiento.firstElementChild.firstElementChild.firstElementChild.value + ",";
                resultado += $scope.convertirFecha(padecimiento.firstElementChild.nextElementSibling.firstElementChild.firstElementChild.value) + "]";
                padecimiento = padecimiento.nextElementSibling;
            }
        }
        resultado += "]";
        console.log(resultado);
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
    $scope.GetAllPedidosNuevos = function (){
        var apiRoute = '/WebApi/consultarPedidosNuevos';
        var respuesta = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        respuesta.then(function (response) {
            $scope.pedidosNuev = response.data;
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.preparado = function (Numpedido) {
        console.log(Numpedido);
        if (confirm("¿Seguro que desea establecer este pedido como preparado?")) {
            var apiRoute = '/WebApi/pedidoPreparado';
            var respuesta = ServicioHTTP.post(apiRoute, Numpedido);
            respuesta.then(function (response) {
                console.log(response.data);
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
        var apiRoute = '/WebApi/consultarPedidosPreparados';
        var respuesta = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        respuesta.then(function (response) {
            $scope.pedidosPrepa = response.data;
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.facturado = function (Numpedido) {
        console.log(Numpedido);
        if (confirm("¿Seguro que desea establecer este pedido como facturado?")) {
            var apiRoute = '/WebApi/pedidoFacturado';
            var respuesta = ServicioHTTP.post(apiRoute, Numpedido);
            respuesta.then(function (response) {
                console.log(response.data);
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
        $scope.pedido =
            {
                fecha_recojo: '20/10/2017',
                numero: '152',
                cliente: '457',
                sucursal: 'ESTA',
                productos: [{ medicamento: "papa", cantidad: '4 kg', precio: '2000' }, { medicamento: "manzana", cantidad: '3.4 kg', precio: '3500' }]
            };
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
        var apiRoute = '/WebApi/consultarPedidosFacturados';
        var respuesta = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        respuesta.then(function (response) {
            $scope.pedidosFactu = response.data;
            console.log(response.data);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    $scope.retirado = function (Numpedido) {
        console.log(Numpedido);
        if (confirm("¿Seguro que desea establecer este pedido como retirado?")) {
            var apiRoute = '/WebApi/pedidoRetirado';
            var respuesta = ServicioHTTP.post(apiRoute, Numpedido);
            respuesta.then(function (response) {
                console.log(response.data);
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
        var apiRoute = '/WebApi/consultarPedidosRetirados';
        var respuesta = ServicioHTTP.getAll(apiRoute, 'authFact.getAccessToken()');
        respuesta.then(function (response) {
            $scope.pedidosReti = response.data;
            console.log(response.data);
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