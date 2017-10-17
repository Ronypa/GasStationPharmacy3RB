var ip = "http://192.168.100.2";

//controlador para la vista principal de los clientes, aqui esta el formulario para realizar los pedidos
app.controller('clientesController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    //$scope.productosSelecccionados;
    $scope.urlMenuClientes = 'AngularJS/Templates/menuClientes.html';
    $scope.hayConReceta = [];
    var fecha = new Date();
    $scope.limitFecha = fecha.getFullYear() + "-" + (fecha.getMonth() + 1) + '-' + fecha.getDate();

    /*funcion para mostrar todos los productos para mostrarlos entre las opciones del pedido*/
    $scope.getProductos = function () {
        var apiRoute = ip + '/WebApi/consultarMedicamentos';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.productos = response.data;
            $scope.interpretarPrescripcion($scope.productos);
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    /*funcion para obtener de la base de datos las recetas que tiene el cliente guardadas*/
    $scope.getRecetasCliente = function () {
        var apiRoute = ip + '/WebApi/consultarRecetas';
        var recetas = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        recetas.then(function (response) {
            $scope.recetasCliente = response.data;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };
    /*funcion para realizar el pedido*/
    $scope.realizarPedido = function () {
        if ($scope.arrayEnvio === undefined || $scope.arrayEnvio.length === 0) {//lo modifique
            alert('Debe seleccionar al menos un producto.');
            return;
        }
        if (8 <= parseInt($scope.horaRecojo.slice(0, 2)) && parseInt($scope.horaRecojo.slice(0, 2)) <= 19) {
            if (!(Math.abs(parseInt($scope.horaRecojo.slice(0, 2)) - fecha.getHours()) >= 4) && $scope.limitFecha === $scope.fechaRecojo) {
                alert('Hora incorrecta');
                return;
            }
        }
        else {
            alert('Hora incorrecta');
            return;
        }
        var pedido =
            {
                nombre: $scope.nombre,
                sucursal: $scope.sucursal,
                telefono: $scope.telefono,
                fecha_recojo: $scope.fechaRecojo + ' ' + $scope.horaRecojo + ":00",
                productos: ''
            };
        var recetasSeleccionadas = $.map($('option[name="opcionesRecetas"]:selected'), function (c) { return c.value; });
        if ($scope.productosConReceta.length >= 1) {
            var pos = 0;
            for (var index = 0; index < $scope.productosConReceta.length; index++) {
                $scope.arrayEnvio[$scope.productosConReceta[index].posicion].receta = parseInt(recetasSeleccionadas[index]);
            }
        }
        pedido.productos = $scope.convertirToString($scope.arrayEnvio);
        var apiRoute = ip + '/WebApi/agregarPedido';//lo mofique
        var respuesta = ServicioHTTP.postToken(apiRoute, pedido, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Pedido realizado.');
        },
            function (error) {
                alert("Error realizando el pedido");
            });
    };

    /*Funcion para convertir el arreglo de productos a string*/
    $scope.convertirToString = function (arreglo) {
        resultado = '[{medicamento:' + '\'' + arreglo[0].medicamento + '\', cantidad:' + arreglo[0].cantidad + ', receta:' + arreglo[0].receta + '}';
        for (var index = 1; index < arreglo.length; index++) {
            resultado += ', {medicamento:' + '\'' + arreglo[index].medicamento + '\', cantidad:' + arreglo[index].cantidad + ', receta:' + arreglo[index].receta + '}';
        }
        return resultado + ']';
    };

    /*funcion para obtener los telefonos del cliente*/
    $scope.getTelefonosCliente = function () {
        var apiRoute = ip + '/WebApi/consultarTelefonosC';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.telefonosCliente = response.data;
        },
            function (error) {
                alert("Error cargando telefonos");
            });
    };

    /*Funcion para obetener los productos que se quieren pedir*/
    $scope.obtenerSeleccionados = function () {
        var numProductos = $.map($('input[name="posiCajaCantidad"]'), function (c) { return c.value; });
        var pos = 0;
        $scope.arrayEnvio = [];
        for (var index = 0; index < numProductos.length; index++) {
            if (numProductos[index] !== "" && numProductos[index] !== '0') {//lo modifique
                var obj = {
                    medicamento: $scope.productos[index].nombre,
                    cantidad: parseInt(numProductos[index]),
                    prescripcion: $scope.productos[index].prescripcion,
                    receta: 0
                };
                $scope.arrayEnvio[pos] = obj;
                pos++;
            }
        }
        //$scope.interpretarPrescripcion($scope.arrayEnvio);
        pos = 0;
        $scope.productosConReceta = [];
        for (var index2 = 0; index2 < $scope.arrayEnvio.length; index2++) {
            if ($scope.arrayEnvio[index2].prescripcion === "Sí") {
                var obj2 = {
                    posicion: index2,
                    medicamento: $scope.arrayEnvio[index2].medicamento
                };
                $scope.productosConReceta[pos] = obj2;
                pos++;
            }
        }        if ($scope.productosConReceta.length >= 1) {
            $scope.hayConReceta = [''];
        }
        else
            $scope.hayConReceta = [];
    };
    /*Obtiene todos los pedidos del cliente*/
    $scope.getPedidosCliente = function () {
        var apiRoute = ip + '/WebApi/consultarPedidos';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.pedidosCliente = response.data;
        },
            function (error) {
                alert("Error cargando los pedidos");
            });
    };

    /*Obtiene los detalles de un pedido del cliente*/
    $scope.getPedidoDetalles = function (numero) {
        var pedido = { opcion: numero.toString() };
        var apiRoute = ip + '/WebApi/consultarDetallePedido';
        var respuesta = ServicioHTTP.postToken(apiRoute, pedido, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.pedido = response.data[0];
            $scope.compania = $scope.pedido.compania;
            $scope.getSucursales();
            $scope.fecha_recojo = $scope.pedido.fecha_recojo.split(" ")[0];
            $scope.hora_recojo = $scope.pedido.fecha_recojo.split(" ")[1];
            if ($scope.pedido.estado === 'n')
                $scope.pedido.estado = 'Nuevo';
            else if ($scope.pedido.estado === 'f')
                $scope.pedido.estado = 'Facturado';
            else if ($scope.pedido.estado === 'p')
                $scope.pedido.estado = 'Preparado';
            else
                $scope.pedido.estado = 'Retirado';
        },
            function (error) {
                alert("Error cargando los pedidos");
            });
    };

    /*Realiza las modificaciones hechas a un pedido del cliente*/
    $scope.aplicarCambios = function () {
        var apiRoute = ip + '/WebApi/actualizarPedido';
        var productosCambiados = $scope.pedido.productos;
        var numProductos = $.map($('input[name="posiCajaCantidadActualizar"]'), function (c) { return c.value; });
        for (var index = 0; index < productosCambiados.length; index++) {
            if (parseInt(numProductos[index]) <= 0) {
                alert('La cantidad del producto no puede ser 0.');
                return;
            }
            productosCambiados[index].cantidad = parseInt(numProductos[index]);
        }
        if ($scope.hora_recojo.length < 6)
            $scope.hora_recojo += ":00";
        var pedidoEditado =
            {
                numero: $scope.pedido.numero,
                nombre: $scope.pedido.nombre,
                sucursal: $scope.pedido.sucursal,
                telefono: $scope.pedido.telefono,
                fecha_recojo: $scope.fecha_recojo + ' ' + $scope.hora_recojo,
                productos: ''
            };
        pedidoEditado.productos = $scope.convertirToString(productosCambiados);
        respuesta = ServicioHTTP.postToken(apiRoute, pedidoEditado, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Pedido actualizado.');
            document.getElementById('botoncerrar').click();
            $scope.getPedidosCliente();
        },
            function (error) {
                alert("Error actualizando el pedido.");
            });

    };

    /*Obtiene los productos de la edicion de un pedido*/
    $scope.obtenerProductosCambios = function () {
        var numProductos = $.map($('input[name="posiCajaCantidadActualizar"]'), function (c) { return c.value; });
        var pos = 0;
        $scope.arrayCambios = [];
        for (var index = 0; index < numProductos.length; index++) {
            if (numProductos[index] !== "" && numProductos[index] !== '0') {//lo modifique
                var obj = {
                    medicamento: $scope.productos[index].nombre,
                    cantidad: parseInt(numProductos[index]),
                    prescripcion: $scope.productos[index].prescripcion,
                    receta: 0
                };
                $scope.arrayEnvio[pos] = obj;
                pos++;
            }
        }
    };

    /*Se obtienen las companias*/
    $scope.getCompanias = function () {
        var apiRoute = ip + '/WebApi/consultarCompanias';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.companias = response.data;
        },
            function (error) {
                alert("Error cargando compañías");
            });
    };

    /*Se obieten las sucursales de la compania*/
    $scope.getSucursales = function () {
        var apiRoute = ip + '/WebApi/consultarSucursales';
        var companiaSeleccionada = {
            opcion: $scope.compania
        };
        var respuesta = ServicioHTTP.postToken(apiRoute, companiaSeleccionada, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.sucursales = response.data;
        },
            function (error) {
                alert("Error cargando sucursales");
            });
    };

    $scope.interpretarPrescripcion = function (productos) {
        for (var index = 0; index < productos.length; index++) {
            if (productos[index].prescripcion === true)
                productos[index].prescripcion = 'Sí';
            else if (productos[index].prescripcion === false)
                productos[index].prescripcion = 'No';
            else if (productos[index].prescripcion === 'Sí')
                productos[index].prescripcion = true;
            else
                productos[index].prescripcion = false;
        }
    };
    $scope.getRecetasCliente();
    setTimeout(() => { $scope.getProductos(); }, 500);
    setTimeout(() => { $scope.getTelefonosCliente(); }, 1000);
    setTimeout(() => { $scope.getCompanias(); }, 1500);
});

//controlador para la vista del formulario de la receta de los clientes
app.controller('clientesRecetasController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuClientes = 'AngularJS/Templates/menuClientes.html';

    /*funcion para obtener de la base de datos las recetas que tiene el cliente guardadas*/
    $scope.getRecetasCliente = function () {
        var apiRoute = ip + '/WebApi/consultarRecetas';
        var recetas = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        recetas.then(function (response) {
            $scope.recetasCliente = response.data;
        },
            function (error) {
                alert("Error cargando recetas");
            });
    };
    //aqui modifique
    /*funcion para obetener los detalles de una receta: nombre,doc,produc,imagen*/
    $scope.detallesReceta = function (numero) {
        var apiRoute = ip + '/WebApi/consultarReceta';
        var receta =
            {
                opcion: numero.toString()
            };
        var recetas = ServicioHTTP.postToken(apiRoute, receta, authFact.getAccessToken());
        recetas.then(function (response) {
            $scope.receta = response.data[0];
            $scope.numeroRec = numero;
            $scope.detallesProductos = $scope.receta.productos;
            $scope.receta.imagen = 'data:image/png;base64,' + $scope.receta.imagen;
        },
            function (error) {
                alert("Error cargando datos");
            });
    };

    /*Obtiene todos los productos de la base de datos y llama a obtenerProductosConPrescripcion*/
    $scope.obtenerProductos = function () {
        var apiRoute = ip + '/WebApi/consultarMedicamentos';
        var respuesta = ServicioHTTP.getAll(apiRoute, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.productos = response.data;
            $scope.obtenerProductosConPrescripcion($scope.productos);
        },
            function (error) {
                alert("Error cargando los productos");
            });
    };

    /*crea un arreglo con todos los productos que tienen prescripcion*/
    $scope.obtenerProductosConPrescripcion = function (productos) {
        $scope.productoParaReceta = [];
        var pos = 0;
        for (var index = 0; index < productos.length; index++) {
            if (productos[index].prescripcion === true) {
                $scope.productoParaReceta[pos] = productos[index];
                pos++;
            }
        }
    };

    //obtiene los prodcutos seleccionados para la receta y la cantidad
    $scope.obtenerProductosSeleccionados = function () {
        var posDeProducto = $.map($('input[name="posiCajaCantidad"]'), function (c) { return c.value; });
        $scope.arrayProductosEnReceta = [];
        var pos = 0;
        for (var index = 0; index < posDeProducto.length; index++) {
            if (posDeProducto[index] !== "" && posDeProducto[index] !== "0") {
                var obj = '{ medicamento: \'' + $scope.productoParaReceta[index].nombre + '\' ,cantidad: ' + parseInt(posDeProducto[index]) + '}';
                $scope.arrayProductosEnReceta[pos] = obj;//esto es lo que se envia en el post
                pos++;
            }
        }
    };

    //obtiene los prodcutos seleccionados para la receta y la cantidad
    $scope.obtenerProductosSeleccionadosEditados = function () {
        var posDeProducto = $.map($('input[name="posiCajaCantidadEditados"]'), function (c) { return c.value; });
        $scope.arrayProductosEnRecetaEditados = [];
        var pos = 0;
        for (var index = 0; index < posDeProducto.length; index++) {
            if (posDeProducto[index] !== "" && posDeProducto[index] !== "0") {
                var obj = '{ medicamento: \'' + $scope.productoParaReceta[index].nombre + '\' ,cantidad: ' + parseInt(posDeProducto[index]) + '}';
                $scope.arrayProductosEnRecetaEditados[pos] = obj;//esto es lo que se envia en el post
                pos++;
            }
        }
    };

    //se envia la receta que se esta creando al servidor
    $scope.agregarReceta = function () {
        if ($scope.arrayProductosEnReceta.length === 0) {
            alert('Debe seleccionar al menos un producto');
            return;
        }
        var apiRoute = ip + '/WebApi/agregarReceta';
        var receta =
            {
                nombre: $scope.nombreReceta,
                numero: $scope.numero,
                doctor: $scope.nombreDoctor,
                medicamentos: '[' + $scope.arrayProductosEnReceta.toString() + ']',
                imagen: $scope.imagen.split(",")[1]
            };
        var respuesta = ServicioHTTP.postToken(apiRoute, receta, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Receta guardada');
            $(".image-preview-filename").val("");
        },
            function (error) {
                alert("Error al crear la receta.");
            });
    };

    /*Aplica los cambios hechos a la receta*/
    $scope.aplicarCambiosReceta = function () {
        var posDeProducto = $.map($('input[name="posiCajaCantidadEditados"]'), function (c) { return c.value; });
        var arrayProductosEnRecetaEditados = [];
        var pos = 0;
        for (var index = 0; index < posDeProducto.length; index++) {
            if (parseInt(posDeProducto[index]) <= 0)
                alert('La cantidad del producto no puede ser cero.');
            var obj = '{ medicamento: \'' + $scope.detallesProductos[index].medicamento + '\' ,cantidad: ' + parseInt(posDeProducto[index]) + '}';
            arrayProductosEnRecetaEditados[pos] = obj;//esto es lo que se envia en el post
            pos++;
        }
        var apiRoute = ip + '/WebApi/actualizarReceta';
        var recetaEditada =
            {
                nombre: $scope.receta.nombre,
                numero: $scope.receta.numero,
                doctor: $scope.receta.doctor,
                medicamentos: '[' + arrayProductosEnRecetaEditados.toString() + ']',
                imagen: $scope.receta.imagen.split(",")[1]
            };
        var respuesta = ServicioHTTP.postToken(apiRoute, recetaEditada, authFact.getAccessToken());
        respuesta.then(function (response) {
            $scope.getRecetasCliente();
            alert('Receta actualizada');
            document.getElementById('botoncerrar').click();
            $(".image-preview-filename").val("");
        },
            function (error) {
                alert("Error al actualizar la receta.");
            });
    };

    //convierte la imagen en un string en base64 para poder enviarla al servidor
    convertirImagen = function (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            $scope.imagen = e.target.result;
        };
        reader.onerror = function (error) {
            alert('Error');
        };
    };

    //convierte la imagen en un string en base64 para poder enviarla al servidor
    convertirImagenEditada = function (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            document.getElementById('imaRec').src = e.target.result;
            $scope.receta.imagen = e.target.result;
        };
        reader.onerror = function (error) {
            alert('Error');
        };
    };



    $scope.obtenerProductos();

    $(document).on('click', '#close-preview', function () {
        $('.image-preview').popover('hide');
        // Hover befor close the preview
        $('.image-preview').hover(
            function () {
                $('.image-preview').popover('show');
            },
            function () {
                $('.image-preview').popover('hide');
            }
        );
    });

    $(function () {
        // Create the close button
        var closebtn = $('<button/>', {
            type: "button",
            text: 'x',
            id: 'close-preview',
            style: 'font-size: initial;'
        });
        closebtn.attr("class", "close pull-right");
        // Set the popover default content
        $('.image-preview').popover({
            trigger: 'manual',
            html: true,
            title: "<strong>Preview</strong>" + $(closebtn)[0].outerHTML,
            content: "There's no image",
            placement: 'bottom'
        });
        // Clear event
        $('.image-preview-clear').click(function () {
            $('.image-preview').attr("data-content", "").popover('hide');
            $('.image-preview-filename').val("");
            $('.image-preview-clear').hide();
            $('.image-preview-input input:file').val("");
            $(".image-preview-input-title").text("Browse");
        });
        // Create the preview image
        $(".image-preview-input input:file").change(function () {
            var img = $('<img/>', {
                id: 'dynamic',
                width: 250,
                height: 200
            });
            var file = this.files[0];
            var reader = new FileReader();
            // Set preview image into the popover data-content
            reader.onload = function (e) {
                $(".image-preview-input-title").text("Cambiar");
                $(".image-preview-clear").show();
                $(".image-preview-filename").val(file.name);
                img.attr('src', e.target.result);
                $(".image-preview").attr("data-content", $(img)[0].outerHTML).popover("show");
            };
            reader.readAsDataURL(file);
        });
    });

});

//controlador para el formulario de actualizacion de datos de los clientes
app.controller('clientesActDatosController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuClientes = 'AngularJS/Templates/menuClientes.html';

    $scope.actualizarDatos = function () {
        var apiRoute = ip + '/WebApi/actualizarDatosPersonales';
        var infoCliente =
            {
                nombre1: $scope.nombre,
                nombre2: $scope.nombre2,
                apellido1: $scope.apellido1,
                apellido2: $scope.apellido2,
                provincia: $scope.provincia,
                ciudad: $scope.ciudad,
                senas: $scope.senas,
                fechaNacimiento: $scope.fechaNacimiento,
                telefono: $scope.telefono
            };
        var respuesta = ServicioHTTP.postToken(apiRoute, infoCliente, authFact.getAccessToken());
        respuesta.then(function (response) {
            alert('Datos actualizados.');
        },
            function (error) {
                alert("Error: algo salió mal.");
            });
    };

    // function to submit the form after all validation has occurred			
    $scope.submitFormActualizar = function () {

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

//controlador para la configuracion de cuenta de la vista de los empleados
app.controller('clientesConfigController', function ($scope, $cookieStore, ServicioHTTP, $location, authFact) {
    $scope.urlMenuClientes = 'AngularJS/Templates/menuClientes.html';

    $scope.submitFormConfig = function () {

        // Set the 'submitted' flag to true
        $scope.submitted = true;

        if ($scope.configForm.$valid) {
            alert("Formulario Valido!");
        }
        else {
            alert("Errores en el formulario!");
        }
    };
});

app.directive('validFile', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attrs, ctrl) {
            ctrl.$setValidity('validFile', el.val() !== '');
            //change event is fired when file is selected
            el.bind('change', function () {
                ctrl.$setValidity('validFile', el.val() !== '');
                scope.$apply(function () {
                    ctrl.$setViewValue(el.val());
                    ctrl.$render();
                });
            });
        }
    };
});


app.directive('ngCompare', function () {
    return {
        require: 'ngModel',
        link: function (scope, currentEl, attrs, ctrl) {
            var comparefield = document.getElementsByName(attrs.ngCompare)[0]; //getting first element
            compareEl = angular.element(comparefield);

            //current field key up
            currentEl.on('keyup', function () {
                if (compareEl.val() !== "") {
                    var isMatch = currentEl.val() === compareEl.val();
                    ctrl.$setValidity('compare', isMatch);
                    scope.$digest();
                }
            });

            //Element to compare field key up
            compareEl.on('keyup', function () {
                if (currentEl.val() !== "") {
                    var isMatch = currentEl.val() === compareEl.val();
                    ctrl.$setValidity('compare', isMatch);
                    scope.$digest();
                }
            });
        }
    };
});



app.directive('ngDropdownRequired', function () {
    return {
        restrict: 'A',
        require: '?ngModel',
        link: function ($scope, elem, attrs, ngModel) {

            $scope.updateForm = function (model, mode) {
                model.$setValidity("requiredSelect", mode);
            };

            $scope.checkModel = function (model) {
                return !((angular.isObject(model) && angular.equals(model, {})) || (angular.isArray(model) && model.length === 0));
            };

            $scope.$watch(attrs.selectedModel, function (val) {
                $scope.updateForm(ngModel, $scope.checkModel(val));
            }, true);
        }
    };
});