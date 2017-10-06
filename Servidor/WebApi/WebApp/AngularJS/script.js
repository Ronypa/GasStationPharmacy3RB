var app = angular.module('app', ['ngRoute','ngCookies']);

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
        controller: "logincontroller",
        authenticated: true
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

app.factory('authFact', ["$cookieStore",function ($cookieStore) {
    var authFact = {};
    authFact.setAccessToken = function (accessToken) {
        $cookieStore.put('accessToken', accessToken);
    };

    authFact.getAccessToken = function () {
        authFact.authToken = $cookieStore.get('accessToken');
        return authFact.authToken;
    };

    return authFact;
}]);

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
app.controller('logincontroller', function ($scope, $cookieStore,ServicioHTTP, $location,authFact) {

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
            $location.path("/entro");
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
            "&grant_type=password"+
            "&client_id=cliente"
        ;

        var apiRoute = '/WebApi/token';
        var respuesta = ServicioHTTP.post(apiRoute, cliente);
        respuesta.then(function (response) {

            var accessToken = response.data.access_token;
            authFact.setAccessToken(accessToken);
            $location.path("/entro");
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

});