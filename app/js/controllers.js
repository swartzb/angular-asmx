'use strict';

/* Controllers */

angular.module('myApp.controllers', [])
  .controller('MyCtrl1', [function() {

  }])
  .controller('MyCtrl2', [function () {

  }])
  .controller('EmployeesController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.selectedEmployee = null;

    $scope.editSelectedEmployee = function () {
      var ndx, len, selectedEmployee;

      for (ndx = 0, len = $scope.employees.length; ndx < len; ++ndx) {
        if ($scope.employees[ndx].isSelected) {
          selectedEmployee = $scope.employees[ndx];
          break;
        }
      }
      if (selectedEmployee) {
        $window.alert('Edit ' + selectedEmployee.LastName + ', ' + selectedEmployee.FirstName);
      }
      return;
    };

    $scope.deleteSelectedEmployee = function () {
      var ndx, len, selectedEmployee;

      for (ndx = 0, len = $scope.employees.length; ndx < len; ++ndx) {
        if ($scope.employees[ndx].isSelected) {
          selectedEmployee = $scope.employees[ndx];
          break;
        }
      }
      if (selectedEmployee) {
        $window.alert('Delete ' + selectedEmployee.LastName + ', ' + selectedEmployee.FirstName);
      }
      return;
    };

    $scope.toggleSelect = function (employee) {
      var ndx, len;

      if (!employee.isSelected) {
        for (ndx = 0, len = $scope.employees.length; ndx < len; ++ndx) {
          $scope.employees[ndx].isSelected = false;
        }
      }
      employee.isSelected = !employee.isSelected;
      $scope.selectedEmployee = employee.isSelected ? employee : null;
      return;
    };

    $http({
      url: '../NwndSvc.asmx/GetAllEmployees',
      method: "POST",
      data: "{}",
      headers: { 'Content-Type': 'application/json' }
    }).success(function (data, status, headers, config) {
      var ndx, len;

      $scope.employees = data.d;
      for (ndx = 0, len = $scope.employees.length; ndx < len; ++ndx) {
        $scope.employees[ndx].isSelected = false;
      }
      return;
    }).error(function (data, status, headers, config) {
      $scope.data = data;
    });
  }]);
