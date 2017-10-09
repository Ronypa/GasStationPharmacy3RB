var app = angular.module('app', ['ui.bootstrap', 'ngRoute', 'ngCookies']);

//Ruteador, (relaciona direccion con html y controlador)
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'AngularJS/Templates/principal.html',
        controller: 'logincontroller'
    })
        .when("/logincliente", {
            templateUrl: 'AngularJS/Templates/sucursal.html',
            ///controller: 'logincontroller'
            controller: "sucursalController"
        })
        .when("/loginadministrador", {
            templateUrl: "AngularJS/Templates/adminClientes.html",
            //controller: "logincontroller"
            controller: "adminCliController"
        })
        .when("/admin", {
            templateUrl: "AngularJS/Templates/adminClientes.html",
            controller: "adminCliController",
            //authenticated: true
        })
        .when("/adminEmp", {
            templateUrl: "AngularJS/Templates/adminEmpleados.html",
            controller: "adminEmpController",
            //authenticated: true
        })
        .when("/adminproductos", {
            templateUrl: "AngularJS/Templates/adminProductos.html",
            controller: "adminProduController",
            //authenticated: true
        })
        .when("/adminroles", {
            templateUrl: "AngularJS/Templates/adminRoles.html",
            controller: "adminRolController",
            //authenticated: true
        })
        .when("/adminsucursales", {
            templateUrl: "AngularJS/Templates/adminSucursales.html",
            controller: "adminSucuController",
            //authenticated: true
        })
        .when("/adminestadisticas", {
            templateUrl: "AngularJS/Templates/adminEstadisticas.html",
            controller: "adminEstaController",
            //authenticated: true
        })
        .when("/registrocliente", {
            templateUrl: "AngularJS/Templates/registrarCliente.html",
            controller: "registercontroller"
        })
        .when("/sucursal", {
            templateUrl: "AngularJS/Templates/sucursal.html",
            controller: "sucursalController",
            //authenticated: true
        })
        .when("/pedidosPreparados", {
            templateUrl: "AngularJS/Templates/pedidosPrepa.html",
            controller: "pedidosPreparadosController",
            //authenticated: true
        })
        .when("/pedidosFacturados", {
            templateUrl: "AngularJS/Templates/pedidosFactu.html",
            controller: "pedidosFacturadosController",
            //authenticated: true
        })
        .when("/pedidosRetirados", {
            templateUrl: "AngularJS/Templates/pedidosReti.html",
            controller: "pedidosRetiradosController",
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