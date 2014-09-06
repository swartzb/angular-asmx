'use strict';


// Declare app level module which depends on filters, and services
angular.module('myApp.routes', ['ngRoute']).
  config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/home', { templateUrl: 'partials/home.html', controller: 'HomeController' });
    $routeProvider.when('/view2', { templateUrl: 'partials/partial2.html', controller: 'MyCtrl2' });
    $routeProvider.when('/employees/load/:loadVal', { templateUrl: 'partials/employees.html', controller: 'EmployeesController' });
    $routeProvider.when('/employees/add', { templateUrl: 'partials/employee.html', controller: 'AddEmployeeController' });
    $routeProvider.when('/employees/edit', { templateUrl: 'partials/employee.html', controller: 'EditEmployeeController' });
    $routeProvider.when('/employees/view', { templateUrl: 'partials/employee.html', controller: 'ViewEmployeeController' });
    $routeProvider.otherwise({ redirectTo: '/home' });
  }]);
