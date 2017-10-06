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