var app = angular.module('app', ['ui.bootstrap', 'ngRoute', 'ngCookies']);

//Ruteador, (relaciona direccion con html y controlador)
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'AngularJS/Templates/principal.html',
        controller: 'logincontroller'
    })
        //redirecciona a la pagina de login de clientes
        .when("/logincliente", {
            templateUrl: 'AngularJS/Templates/loginCliente.html',
            controller: 'logincontroller'
            //controller: "sucursalController"
            //controller: "clientesController"
        })
        //redirecciona a la pagina de login de administradores
        .when("/loginadministrador", {
            templateUrl: "AngularJS/Templates/loginAdministrador.html",
            controller: "logincontroller"
            //controller: "adminCliController"
        })
        //redirecciona a la vista principal de administracion donde por defecto aparace la administracion de clientes
        .when("/admin", {
            templateUrl: "AngularJS/Templates/adminClientes.html",
            controller: "adminCliController"
            //authenticated: true
        })
        //redirecciona a la vista de administracion de empleados
        .when("/adminEmp", {
            templateUrl: "AngularJS/Templates/adminEmpleados.html",
            controller: "adminEmpController"
            //authenticated: true
        })
        //redirecciona a la vista de administracion de productos
        .when("/adminproductos", {
            templateUrl: "AngularJS/Templates/adminProductos.html",
            controller: "adminProduController"
            //authenticated: true
        })
        //redirecciona a la vista de administracion de roles
        .when("/adminroles", {
            templateUrl: "AngularJS/Templates/adminRoles.html",
            controller: "adminRolController"
            //authenticated: true
        })
        //redirecciona a la vista de administracion de sucursales
        .when("/adminsucursales", {
            templateUrl: "AngularJS/Templates/adminSucursales.html",
            controller: "adminSucuController"
            //authenticated: true
        })
        //redirecciona a la vista de administracion de estadisticas
        .when("/adminestadisticas", {
            templateUrl: "AngularJS/Templates/adminEstadisticas.html",
            controller: "adminEstaController"
            //authenticated: true
        })
        //redirecciona a la pagina donde se registran los clientes
        .when("/registrocliente", {
            templateUrl: "AngularJS/Templates/registrarCliente.html",
            controller: "registercontroller"
        })
        //redirecciona a la vista principal de la sucursal, por defecto aqui aparecen los pedidos nuevos regitrados
        .when("/sucursal", {
            templateUrl: "AngularJS/Templates/sucursal.html",
            controller: "sucursalController"
            //authenticated: true
        })
        //redirecciona a la vista sucursal de los pedido privados
        .when("/pedidosPreparados", {
            templateUrl: "AngularJS/Templates/pedidosPrepa.html",
            controller: "pedidosPreparadosController"
            //authenticated: true
        })
        //redirecciona a la vista sucursal de los pedidos facturados
        .when("/pedidosFacturados", {
            templateUrl: "AngularJS/Templates/pedidosFactu.html",
            controller: "pedidosFacturadosController"
            //authenticated: true
        })
        //redirecciona a la vista sucursal de los pedidos ya retirados
        .when("/pedidosRetirados", {
            templateUrl: "AngularJS/Templates/pedidosReti.html",
            controller: "pedidosRetiradosController"
            //authenticated: true
        })
        //redirecciona a la vista principal de clientes donde aparece el formulario para realizar pedido
        .when("/clientesPedidos", {
            templateUrl: "AngularJS/Templates/clientes.html",
            controller: "clientesController"
            //authenticated: true
        })
        //redirecciona a la vista clientes donde esta el formulario para las recetas
        .when("/clientesRecetas", {
            templateUrl: "AngularJS/Templates/clientesRecetas.html",
            controller: "clientesRecetasController"
            //authenticated: true
        })
        //redirecciona a la vista clientes donde esta el formulario de actualizacion de datos
        .when("/clientesActDatos", {
            templateUrl: "AngularJS/Templates/clientesActDatos.html",
            controller: "clientesActDatosController"
            //authenticated: true
        })
        //redirecciona a la vista clientes donde esta el formulario para la configuracion
        .when("/clientesConfig", {
            templateUrl: "AngularJS/Templates/clientesConfig.html",
            controller: "clientesConfigController"
            //authenticated: true
        })
        .otherwise({
            redirectTo: '/'
        });
});

app.run(["$rootScope", "$location", "authFact", function ($rootScope,
    $location, authFact) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (next.$$route.authenticated) {
            var userAuth = authFact.getAccessToken();
            if (!userAuth) {
                $location.path('/');
            }
        }
    });
}]);