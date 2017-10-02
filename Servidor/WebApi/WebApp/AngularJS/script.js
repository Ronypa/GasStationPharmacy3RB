var app = angular.module('app', ['ngRoute']);

//Ruteador, (relaciona direccion con html y controlador)
app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'AngularJS/Templates/principal.html',
        controller: 'logincontroller'
    })
        .when("/logincliente", {
            templateUrl: 'AngularJS/Templates/loginCliente.html',
            controller: 'logincontroller'
        })
        .when("/loginadministrador", {
            templateUrl: "AngularJS/Templates/loginAdministrador.html",
            controller: "logincontroller"
        })
        .when("/entro", {
            templateUrl: "AngularJS/Templates/entro.html",
            controller: "logincontroller"
        })
        .otherwise({
            redirectTo: '/'
        });
});

//Servicio para realizar peticiones http 
app.service('ServicioHTTP', function ($http) {
    var urlGet = '';
    this.post = function (apiRoute, Model) {
        var request = $http({
            method: "post",
            url: apiRoute,
            data: Model
        });
        return request;
    };
    this.put = function (apiRoute, Model) {
        var request = $http({
            method: "put",
            url: apiRoute,
            data: Model
        });
        return request;
    };
    this.delete = function (apiRoute) {
        var request = $http({
            method: "delete",
            url: apiRoute
        });
        return request;
    };
    this.getAll = function (apiRoute) {
        urlGet = apiRoute;
        return $http.get(urlGet);
    };

    this.getLogin = function (apiRoute, cedula, contrasena) {
        urlGet = apiRoute + '/' + cedula + '/' + contrasena;
        return $http.get(urlGet);
    };
});


//Controlador para el login tiene la funcion que se llama desde el html (hace uso del servicio "loginService")
app.controller('logincontroller', function ($scope, ServicioHTTP, $location) {

    $scope.loginAdministrador = function () {
        var apiRoute = '/WebApi/api/EMPLEADO';
        var student = ServicioHTTP.getLogin(apiRoute, $scope.loginCedula, $scope.loginPassword);
        student.then(function (response) {
            $location.path("/entro");
        },
            function (error) {
                alert("Usuario y/o contraseña incorrecto");
            });
    };

    $scope.loginCliente = function () {
        var apiRoute = '/WebApi/api/Cliente';
        var student = ServicioHTTP.getLogin(apiRoute, $scope.loginCedula, $scope.loginPassword);
        student.then(function (response) {
            $location.path("/entro");
        },
            function (error) {
                alert("Usuario y/o contraseña incorrecto");
            });
    };

    //funciones para redireccionar a las vistas de log in
    $scope.loginadministrador = function () {
        $location.path("/loginadministrador");
    };
    $scope.logincliente = function () {
        $location.path("/logincliente");
    };

});