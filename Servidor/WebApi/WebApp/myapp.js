// creating the module and giving it the name myapp 'which is in our html ng-app'
var myApp = angular.module('myapp', ['ngRoute']);

// configure our routes
myApp.config(function ($routeProvider) {
    $routeProvider


        // route for the home page
        .when('/', {
            //This is to define the templates in our html file
            templateUrl: 'Paginas/Clientes.html',
            controller: 'main'
        })

        // route for the about page
        .when('/doctores', {
            templateUrl: 'Paginas/Doctores.html',
            controller: 'doctores'
        })

        // route for the contact page
        .when('/medicamentos', {
            templateUrl: 'Paginas/Medicamentos.html',
            controller: 'medicamentos'
        })
        // route for the about page
        .when('/roles', {
            templateUrl: 'Paginas/Roles.html',
            controller: 'roles'
        })

        // route for the contact page
        .when('/sucursales', {
            templateUrl: 'Paginas/Sucursales.html',
            controller: 'sucursales'
        })

        // route for the contact page
        .when('/estadisticas', {
            templateUrl: 'Paginas/Estadisticas.html',
            controller: 'estadisticas'
        });


});

// creating the controller and also injecting Angular's $scope
	myApp.controller('main', function($scope) {
		// create a message to display in our view
		$scope.message = 'What appears in the Home Page';
	});

// creating the controller and also injecting Angular's $scope
myApp.controller('clientes', function ($scope) {
    // create a message to display in our view
    $scope.message = 'What appears in the Home Page';
});

myApp.controller('doctores', function ($scope) {
    $scope.message = 'The contenet in the About page.';
});

myApp.controller('medicamentos', function ($scope) {
    $scope.message = 'This is our News Feed.';
});

// creating the controller and also injecting Angular's $scope
myApp.controller('roles', function ($scope) {
    // create a message to display in our view
    $scope.message = 'What appears in the Home Page';
});

myApp.controller('sucursales', function ($scope) {
    $scope.message = 'The contenet in the About page.';
});

myApp.controller('estadisticas', function ($scope) {
    $scope.message = 'This is our News Feed.';
});