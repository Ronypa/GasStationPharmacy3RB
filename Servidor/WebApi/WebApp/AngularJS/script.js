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
        // route for the home page
        .when('/admin', {
            //This is to define the templates in our html file
            templateUrl: 'AngularJS/Templates/Clientes.html',
            controller: 'clientes'
        })

        // route for the about page
        .when('/doctores', {
            templateUrl: 'AngularJS/Templates/Doctores.html',
            controller: 'doctores'
        })

        // route for the contact page
        .when('/medicamentos', {
            templateUrl: 'AngularJS/Templates/Medicamentos.html',
            controller: 'medicamentos'
        })
        // route for the about page
        .when('/roles', {
            templateUrl: 'AngularJS/Templates/Roles.html',
            controller: 'roles'
        })

        // route for the contact page
        .when('/sucursales', {
            templateUrl: 'AngularJS/Templates/sucursales.html',
            controller: 'sucursales'
        })

        // route for the contact page
        .when('/estadisticas', {
            templateUrl: 'AngularJS/Templates/estadisticas.html',
            controller: 'estadisticas'
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
app.controller('logincontroller', function ($scope, $rootScope, ServicioHTTP, $location) {
    $rootScope.hideit = true;
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

// creating the controller and also injecting Angular's $scope
app.controller('clientes', function ($scope, $rootScope) {
    // create a message to display in our view
    $rootScope.hideit = false;
    $scope.message = 'What appears in the Home Page';
});

app.controller('doctores', function ($scope, $rootScope) {
    $rootScope.hideit = false;
    $scope.message = 'The contenet in the About page.';
});

app.controller('medicamentos', function ($scope, $rootScope) {
    $rootScope.hideit = false;
    $scope.message = 'This is our News Feed.';
});

// creating the controller and also injecting Angular's $scope
app.controller('roles', function ($scope, $rootScope) {
    $rootScope.hideit = false;
    // create a message to display in our view
    $scope.message = 'What appears in the Home Page';
});

app.controller('sucursales', function ($scope, $rootScope) {
    $rootScope.hideit = false;
    $scope.message = 'The contenet in the About page.';
});

app.controller('estadisticas', function ($scope, $rootScope) {
    $rootScope.hideit = false;
    $scope.message = 'This is our News Feed.';
});