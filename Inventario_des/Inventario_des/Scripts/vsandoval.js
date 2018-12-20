/* vsandoval.js v0.0.2
 * Copyright © 2017 Victor Sandoval Norambueba
 * Free to use under the WTFPL license.
 * victorssandovaln@gmail.com
 */



$(function () {

    if (document.getElementById("listacuentasadid") == null) {
    }
    else {
        // Cuentas Active Directory

        var cuentas_ad = document.getElementById("listacuentasadid");
        //auxiliar.type = "text";
        var lista_cuentas_ad = cuentas_ad.value;
        lista_cuentas_ad = lista_cuentas_ad.replace(/'/g, '"');
        var array_lista_cuentas_ad = JSON.parse("[" + lista_cuentas_ad + "]");

        var token_cuentas_ad = new Bloodhound({
            //datumTokenizer: Bloodhound.tokenizers.obj.whitespace('email', 'nombre'),

            datumTokenizer: function (ds) {

                var uno = Bloodhound.tokenizers.whitespace(ds.email);
                var dos = Bloodhound.tokenizers.whitespace(ds.nombre);
                //return uno.concat(dos);

                //var test_ds = Bloodhound.tokenizers.whitespace(ds.email, ds.nombre);
                var test_ds = uno.concat(dos);

                $.each(test_ds, function (k, v) {
                    i = 0;
                    while ((i + 1) < v.length) {
                        test_ds.push(v.substr(i, v.length));
                        i++;
                    }
                })
                return test_ds;
            },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: array_lista_cuentas_ad
        });

        token_cuentas_ad.initialize();

        // Gerente
        if (document.getElementById("emailgerenteid") != null) {
            $('#divemailgerente .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailgerente .typeahead').val(data.email).trigger('keyup');
                });
        
            var myVal = $('#emailgerenteid').val();
            $('#divemailgerente .typeahead').val(myVal).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }

        // Solicitante
        if (document.getElementById("solicitanteid") != null) {
            $('#divemailsolicitante .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailsolicitante .typeahead').val(data.email).trigger('keyup');
                });
            
            var myVal = $('#solicitanteid').val();
            $('#divemailsolicitante .typeahead').val(myVal).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }

        // Usuario Final
        if (document.getElementById("emailusuariofinalid") != null) {
            $('#divemailusuariofinal .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailusuariofinal .typeahead').val(data.email).trigger('keyup');
                });
        
            var myVal = $('#emailusuariofinalid').val();
            $('#divemailusuariofinal .typeahead').val(myVal).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }

        // Técnico PREparación
        if (document.getElementById("emailtecnicopreid") != null) {
            $('#divemailtecnicopre .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailtecnicopre .typeahead').val(data.email).trigger('keyup');
                });

            var myValPreId = $('#emailtecnicopreid').val();
            $('#divemailtecnicopre .typeahead').val(myValPreId).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }


        // Técnico ENTrega
        if (document.getElementById("emailtecnicoentid") != null) {
            $('#divemailtecnicoent .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailtecnicoent .typeahead').val(data.email).trigger('keyup');
                });

            var myValEntId = $('#emailtecnicoentid').val();
            $('#divemailtecnicoent .typeahead').val(myValEntId).trigger('keyup');
            //$('#emailtecnicoentid').typeahead('val', myValEntId).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }


        // Quien Recibe
        if (document.getElementById("emailquienrecibeid") != null) {
            $('#divemailquienrecibe .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailquienrecibe .typeahead').val(data.email).trigger('keyup');
                });

            var myValEQR1 = $('#emailquienrecibeid').val();
            $('#divemailquienrecibe .typeahead').val(myValEQR1).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }


        // Quien Retira
        if (document.getElementById("emailquienretiraid") != null) {
            $('#divemailquienretira .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'cuentas_ad',
                    displayKey: 'email',
                    source: token_cuentas_ad.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe email en Active Directory. ',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.email + '– <em>' + data.nombre + '</em></div>'
                        }
                    }
                }).on('typeahead:selected', function (event, data) {
                    $('#divemailquienretira .typeahead').val(data.email).trigger('keyup');
                });

            var myValEQR2 = $('#emailquienretiraid').val();
            $('#divemailquienretira .typeahead').val(myValEQR2).trigger('keyup');
            //$('#modificar_colaboradoremailid').typeahead('val', myVal).trigger('keyup');
            //$('#divcolaboradoremail .typeahead').typeahead('val', myVal).trigger('keyup');
        }
    }



    if (document.getElementById("listamarcasid") == null) {
        // nada por ahora
    }
    else {
        var marcas = document.getElementById("listamarcasid");
        //auxiliar.type = "text";
        var lista_marcas = marcas.value;
        lista_marcas = lista_marcas.replace(/'/g, '"');
        var array_lista_marcas = JSON.parse("[" + lista_marcas + "]");

        // Solicitante
        $('#divequipomarca .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'marca',
                val: 'test',
                //empty: "NA",
                source: substringMatcher(array_lista_marcas),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'No existe Marca. Favor escribir Otra.',
                        '</div>'
                    ].join('\n')
                }
            });

        if (document.getElementById("equipo_marca_id") != null) {
            var myValEM = $('#equipo_marca_id').val();
            $('#divequipomarca .typeahead').val(myValEM).trigger('keyup');
        }
    }


    if (document.getElementById("listaTEid") == null) {
        // nada por ahora
    }
    else {
        var tipoequipo = document.getElementById("listaTEid");
        //auxiliar.type = "text";
        var lista_tipoequipo = tipoequipo.value;
        lista_tipoequipo = lista_tipoequipo.replace(/'/g, '"');
        var array_lista_tipoequipo = JSON.parse("[" + lista_tipoequipo + "]");

        // tipo equipo
        $('#divequipotipo .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'tipoequipo',
                val: 'test',
                //empty: "NA",
                source: substringMatcher(array_lista_tipoequipo),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'No existe el Tipo Equipo redactado. Favor escribir otro.',
                        '</div>'
                    ].join('\n')
                }
            });

        if (document.getElementById("equipo_tipo_id") != null) {
            var myValET = $('#equipo_tipo_id').val();
            $('#divequipotipo .typeahead').val(myValET).trigger('keyup');
        }
    }
    

    if (document.getElementById("fechaguiaid") == null) {
        // nada por ahora
    }
    else {
        $("#fechaguiaid").datepicker({
            //showOn: 'both',
            //buttonImage: 'calendar.png',
            //buttonImageOnly: true,
            //changeYear: true,
            //numberOfMonths: 2
            format: 'dd/mm/yyyy',
            startDate: '01/01/2018',
            endDate: '30/12/3020',
            autoclose: true,
            weekStart: 1,
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        });
    }


    if (document.getElementById("fechaentregaid") == null) {
        // nada por ahora
    }
    else {
        $("#fechaentregaid").datepicker({
            //showOn: 'both',
            //buttonImage: 'calendar.png',
            //buttonImageOnly: true,
            //changeYear: true,
            //numberOfMonths: 2
            format: 'dd/mm/yyyy',
            startDate: '01/01/2018',
            endDate: '30/12/3020',
            autoclose: true,
            weekStart: 1,
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        });
    }


    if (document.getElementById("fecharetiroid") == null) {
        // nada por ahora
    }
    else {
        $("#fecharetiroid").datepicker({
            //showOn: 'both',
            //buttonImage: 'calendar.png',
            //buttonImageOnly: true,
            //changeYear: true,
            //numberOfMonths: 2
            format: 'dd/mm/yyyy',
            startDate: '01/01/2018',
            endDate: '30/12/3020',
            autoclose: true,
            weekStart: 1,
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        });
    }


    if (document.getElementById("listaproyectosid") == null) {
        // nada por ahora
    }
    else {
        var proyectos = document.getElementById("listaproyectosid");
        //auxiliar.type = "text";
        var lista_proyectos = proyectos.value;
        lista_proyectos = lista_proyectos.replace(/'/g, '"');
        var array_lista_proyectos = JSON.parse("[" + lista_proyectos + "]");

        var token_proyectos = new Bloodhound({
            //datumTokenizer: Bloodhound.tokenizers.obj.whitespace('email', 'nombre'),

            datumTokenizer: function (ds) {

                var uno = Bloodhound.tokenizers.whitespace(ds.proyecto_nombre);
                var dos = Bloodhound.tokenizers.whitespace(ds.macroempresa_id);
                var tres = Bloodhound.tokenizers.whitespace(ds.razonsocial_nombre);
                var cuatro = Bloodhound.tokenizers.whitespace(ds.centrocosto_nombre);
                var cinco = Bloodhound.tokenizers.whitespace(ds.ubicacion_id);
                var seis = Bloodhound.tokenizers.whitespace(ds.comuna_nombre);
                var siete = Bloodhound.tokenizers.whitespace(ds.direccion_nombre);
                var ocho = Bloodhound.tokenizers.whitespace(ds.ubicacion_nombre);
                //return uno.concat(dos);

                //var test_ds = Bloodhound.tokenizers.whitespace(ds.email, ds.nombre);
                var test_ds = uno.concat(dos).concat(tres).concat(cuatro).concat(cinco).concat(seis).concat(siete).concat(ocho);

                $.each(test_ds, function (k, v) {
                    i = 0;
                    while ((i + 1) < v.length) {
                        test_ds.push(v.substr(i, v.length));
                        i++;
                    }
                })
                return test_ds;
            },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: array_lista_proyectos
        });

        token_proyectos.initialize();

        if (document.getElementById("proyectoid") != null) {

            // Proyecto -> carga otros campos
            $('#divproyecto .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'proyecto',
                    displayKey: 'proyecto_nombre',
                    source: token_proyectos.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe Proyecto. Favor seleccionar otro.',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.proyecto_nombre + '– <em>' + data.ubicacion_nombre + '- ' + data.centrocosto_nombre + '</em></div>'
                        }
                    }
            }).on('typeahead:selected', function (event, data) {
                $('#divproyecto .typeahead').val(data.proyecto_nombre).trigger('keyup');

                $('#macroempresaid').val(data.macroempresa_id).trigger('keyup');
                //$('#empresarazonsocialid').val(data.razonsocial_nombre).trigger('keyup');
                $('#empresarazonsocialid').typeahead('val', data.razonsocial_nombre).trigger('keyup');
                //$('#centrocostoid').val(data.centrocosto_nombre).trigger('keyup');
                $('#centrocostoid').typeahead('val', data.centrocosto_nombre).trigger('keyup');
                $('#ubicacionid').val(data.ubicacion_id).trigger('keyup');
                //$('#comunaid').val(data.comuna_nombre).trigger('keyup');
                $('#comunaid').typeahead('val', data.comuna_nombre).trigger('keyup');
                //$('#direccionid').val(data.direccion_nombre).trigger('keyup');
                $('#direccionid').typeahead('val', data.direccion_nombre).trigger('keyup');
                });

            //if (document.getElementById("proyectoid") != null) {
                var myVal1 = $('#proyectoid').val();
                $('#divproyecto .typeahead').val(myVal1).trigger('keyup');
                //$('#modificar_solicitanteemailid').typeahead('val', myVal).trigger('keyup');
            //}

            if (document.getElementById("macroempresaid") != null)
            {
                var myVal0 = $('#macroempresaid').val();
                $('#macroempresaid').val(myVal0).trigger('keyup');
            }            

            if (document.getElementById("empresarazonsocialid") != null) {
                var myVal2 = $('#empresarazonsocialid').val();
                $('#empresarazonsocialid').val(myVal2).trigger('keyup');
            }

            if (document.getElementById("centrocostoid") != null) {
                var myVal3 = $('#centrocostoid').val();
                $('#centrocostoid').val(myVal3).trigger('keyup');
            }

            if (document.getElementById("ubicacionid") != null) {
                var myVal4 = $('#ubicacionid').val();
                $('#ubicacionid').val(myVal4).trigger('keyup');
            }

            if (document.getElementById("comunaid") != null) {
                var myVal6 = $('#comunaid').val();
                $('#comunaid').val(myVal6).trigger('keyup');
            }

            if (document.getElementById("direccionid") != null) {
                var myVal5 = $('#direccionid').val();
                $('#direccionid').val(myVal5).trigger('keyup');
            }
        }
    }


    $('#proyectoid').change(function () {

        var myValPro = $('#proyectoid').val();

        if (document.getElementById("macroempresaid") != null) {
            var myValMac = $('#macroempresaid').val();

            if (myValPro != null && myValPro != '') {
                $('#macroempresaid').val(myValMac).trigger('keyup');
            } else {
                $('#macroempresaid').val(null).trigger('keyup');
            }
        }

        if (document.getElementById("empresarazonsocialid") != null) {
            var myValRaz = $('#empresarazonsocialid').val();

            if (myValPro != null && myValPro != '') {
                //$('#empresarazonsocialid').val(myValRaz).trigger('keyup');
                $('#empresarazonsocialid').typeahead('val', myValRaz).trigger('keyup');
            } else {
                //$('#empresarazonsocialid').val(null).trigger('keyup');
                $('#empresarazonsocialid').typeahead('val', null).trigger('keyup');
            }
        }

        if (document.getElementById("centrocostoid") != null) {
            var myValCen = $('#centrocostoid').val();

            if (myValPro != null && myValPro != '') {
                //$('#centrocostoid').val(myValCen).trigger('keyup');
                $('#centrocostoid').typeahead('val', myValCen).trigger('keyup');
            } else {
               // $('#centrocostoid').val(null).trigger('keyup');
                $('#centrocostoid').typeahead('val', null).trigger('keyup');
            }
        }

        if (document.getElementById("ubicacionid") != null) {
            var myValUbi = $('#ubicacionid').val();

            if (myValPro != null && myValPro != '') {
                $('#ubicacionid').val(myValUbi).trigger('keyup');
            } else {
                $('#ubicacionid').val(null).trigger('keyup');
            }
        }

        if (document.getElementById("comunaid") != null) {
            var myValCom = $('#comunaid').val();

            if (myValPro != null && myValPro != '') {
                //$('#comunaid').val(myValCom).trigger('keyup');
                $('#comunaid').typeahead('val', myValCom).trigger('keyup');
            } else {
                //$('#comunaid').val(null).trigger('keyup');
                $('#comunaid').typeahead('val', null).trigger('keyup');
            }
        }

        if (document.getElementById("direccionid") != null) {
            var myValDir = $('#direccionid').val();

            if (myValPro != null && myValPro != '') {
                //$('#direccionid').val(myValDir).trigger('keyup');
                $('#direccionid').typeahead('val', myValDir).trigger('keyup');
            } else {
                //$('#direccionid').val(null).trigger('keyup');
                $('#direccionid').typeahead('val', null).trigger('keyup');
            }
        }
        
    });



    if (document.getElementById("listacomunasid") == null) {
        // nada por ahora
    }
    else {
        var comunas = document.getElementById("listacomunasid");
        //auxiliar.type = "text";
        var lista_comunas = comunas.value;
        lista_comunas = lista_comunas.replace(/'/g, '"');
        var array_lista_comunas = JSON.parse("[" + lista_comunas + "]");

        // Solicitante
        $('#divcomuna .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'comuna',
                val: 'test',
                //empty: "NA",
                source: substringMatcher(array_lista_comunas),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'No existe Comuna. Favor seleccionar otra.',
                        '</div>'
                    ].join('\n')
                }
            });

        if (document.getElementById("comunaid") != null) {
            var myValCOM = $('#comunaid').val();
            $('#divcomuna .typeahead').val(myValCOM).trigger('keyup');
        }
    }


    if (document.getElementById("listarsid") == null) {
    }
    else {
        var rs_empresas = document.getElementById("listarsid");
        //auxiliar.type = "text";
        var lista_rs_empresas = rs_empresas.value;
        lista_rs_empresas = lista_rs_empresas.replace(/'/g, '"');
        var array_lista_rs_empresas = JSON.parse("[" + lista_rs_empresas + "]");

        var token_rs_empresas = new Bloodhound({
            //datumTokenizer: Bloodhound.tokenizers.obj.whitespace('razonsocial'),

            datumTokenizer: function (ds) {

                var uno = Bloodhound.tokenizers.whitespace(ds.rut);
                var dos = Bloodhound.tokenizers.whitespace(ds.razonsocial);
                //return uno.concat(dos);

                //var test_ds = Bloodhound.tokenizers.whitespace(ds.email, ds.nombre);
                var test_ds = uno.concat(dos);

                $.each(test_ds, function (k, v) {
                    i = 0;
                    while ((i + 1) < v.length) {
                        test_ds.push(v.substr(i, v.length));
                        i++;
                    }
                })
                return test_ds;
            },

            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: array_lista_rs_empresas
        });

        token_rs_empresas.initialize();

        // RS
        if (document.getElementById("empresarazonsocialid") != null) {
            $('#divempresarazonsocial .typeahead').typeahead(
                {
                    hint: true,
                    highlight: true,
                    minLength: 1
                }, {
                    name: 'rsempresas',
                    displayKey: 'razonsocial',
                    source: token_rs_empresas.ttAdapter(),
                    limit: 10,
                    templates: {
                        empty: [
                            '<div class="empty-message">',
                            'No existe Razón Social de la Empresa.',
                            '</div>'
                        ].join('\n'),
                        suggestion: function (data) {
                            return '<div>' + data.razonsocial + '– <em>' + data.rut + '</em></div>'
                        }
                    }
            }).on('typeahead:selected', function (event, data) {
                $('#empresarazonsocialid .typeahead').val(data.razonsocial).trigger('keyup');
            });
        
            var myValERS = $('#empresarazonsocialid').val();
            $('#divempresarazonsocial .typeahead').val(myValERS).trigger('keyup');
            //$('#modificar_empresarazonsocialid').typeahead('val', myVal); 
            //$('#modificar_empresarazonsocialid').typeahead('val', myVal).trigger('keyup');
        }
    }
    

    if (document.getElementById("listaccid") == null) {
        // nada por ahora
    }
    else {
        var cc = document.getElementById("listaccid");
        //auxiliar.type = "text";
        var lista_cc = cc.value;
        lista_cc = lista_cc.replace(/'/g, '"');
        var array_lista_cc = JSON.parse("[" + lista_cc + "]");

        // Solicitante
        $('#divcentrocosto .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'centrocosto',
                val: 'test',
                //empty: "NA",
                source: substringMatcher(array_lista_cc),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'No existe Centro Costo. Favor seleccionar otro.',
                        '</div>'
                    ].join('\n')
                }
            });

        if (document.getElementById("centrocostoid") != null) {
            var myValCC = $('#centrocostoid').val();
            $('#divcentrocosto .typeahead').val(myValCC).trigger('keyup');
        }
    }


    if (document.getElementById("listadireccionesid") == null) {
        // nada por ahora
    }
    else {
        var direcciones = document.getElementById("listadireccionesid");
        //auxiliar.type = "text";
        var lista_direcciones = direcciones.value;
        lista_direcciones = lista_direcciones.replace(/'/g, '"');
        var array_lista_direcciones = JSON.parse("[" + lista_direcciones + "]");

        // Direccion
        $('#divdireccion .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'direccion',
                //val: 'test',
                //empty: "NA",
                source: substringMatcher(array_lista_direcciones),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'No existe Dirección. Favor seleccionar otra.',
                        '</div>'
                    ].join('\n')
                }
            });

        if (document.getElementById("direccionid") != null) {
            var myValDIR = $('#direccionid').val();
            $('#divdireccion .typeahead').val(myValDIR).trigger('keyup');
        }
    }


    if (document.getElementById("listasoid") == null) {
        // nada por ahora
    }
    else {
        var sistemasoperativos = document.getElementById("listasoid");
        //auxiliar.type = "text";
        var lista_so = sistemasoperativos.value;
        lista_so = lista_so.replace(/'/g, '"');
        var array_lista_so = JSON.parse("[" + lista_so + "]");

        // Solicitante
        $('#divsistemaoperativo .typeahead').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },
            {
                name: 'sistemaoperativo',
                //val: 'test',
                //empty: "NA",
                source: substringMatcher(array_lista_so),
                templates: {
                    empty: [
                        '<div class="empty-message">',
                        'No existe Sistema Operativo. Favor escribir Otro.',
                        '</div>'
                    ].join('\n')
                }
            });

        if (document.getElementById("sistemaoperativoid") != null) {
            var myValSiOp = $('#sistemaoperativoid').val();
            $('#divsistemaoperativo .typeahead').val(myValSiOp).trigger('keyup');
        }
    }

    
    // We can attach the `fileselect` event to all file inputs on the page
    $(document).on('change', ':file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });

    // We can watch for our custom `fileselect` event like this
    $(document).ready(function () {
        $(':file').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' documentos' : label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }
        });
    });


    $(document).ready(function () {
        $('#myTable').dataTable({
            pageLength: 50//,
            //order: [0, 'desc']
        });
    });


    $("#btnAgregarEquipo").click(function (e) {

        //var itemIndex = $("#myTable input.iHidden").length;
        //e.preventDefault();
        ////var newItem = $("<tr><td><input id='Interests_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='Interests[" + itemIndex + "].Id' /><input type='text' id='Interests_" + itemIndex + "__InterestText' name='Interests[" + itemIndex + "].InterestText'/></td><td><input type='checkbox' value='true'  id='Interests_" + itemIndex + "__IsExperienced' name='Interests[" + itemIndex + "].IsExperienced' /></tr>");
        ////var newItem = $("<tr><td><input id='Equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='Equipo[" + itemIndex + "].Id' /><input type='text' id='Equipos_" + itemIndex + "__tiponombre' name='Equipo[" + itemIndex + "].tiponombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__marcanombre' name='Equipo[" + itemIndex + "].marcanombre'/><input type='text' id='Equipos_" + itemIndex + "__modelonombre' name='Equipo[" + itemIndex + "].modelonombre'/><input type='text' id='Equipos_" + itemIndex + "__numeroserie' name='Equipo[" + itemIndex + "].numeroserie'/></tr>");
        ////var newItem = $("<tr><td><input id='Equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='Equipo[" + itemIndex + "].Id' /><input type='text' id='Equipos_" + itemIndex + "__tiponombre' name='Equipo[" + itemIndex + "].tiponombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__marcanombre' name='Equipo[" + itemIndex + "].marcanombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__modelonombre' name='Equipo[" + itemIndex + "].modelonombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__numeroserie' name='Equipo[" + itemIndex + "].numeroserie'/></td></tr>");
        ////var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td>@Html.EditorFor(modelItem => equipo.MarcaNombre, new { htmlAttributes = new { @id = 'equipos_" + itemIndex + "__MarcaNombre' @name = 'equipos[" + itemIndex + "].MarcaNombre' } })</td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td></tr>");
        ////var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__MarcaNombre' name='equipos[" + itemIndex + "].MarcaNombre' /></td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td></tr>");
        ////var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__MarcaNombre' name='equipos[" + itemIndex + "].MarcaNombre' /></td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td><td><input type='checkbox' value='false' id='equipos_" + itemIndex + "__Bolso' name='equipos[" + itemIndex + "].Bolso'/></td><td><input type='checkbox' value='false' id='equipos_" + itemIndex + "__Mouse' name='equipos[" + itemIndex + "].Mouse'/></td><td><input type='checkbox' id='equipos_" + itemIndex + "__Candado' name='equipos[" + itemIndex + "].Candado'/></td></tr>");
        //var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td><select id='equipos_" + itemIndex + "__MarcaNombre' name='equipos[" + itemIndex + "].MarcaNombre'><option value='hp'>HP</option><option value='acer'>ACER</option><option value='aoc'>AOC</option><option value='apple'>APPLE</option></select></td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td><td><input type='checkbox' value='false' id='equipos_" + itemIndex + "__Bolso' name='equipos[" + itemIndex + "].Bolso'/></td><td><input type='checkbox' value='false' id='equipos_" + itemIndex + "__Mouse' name='equipos[" + itemIndex + "].Mouse'/></td><td><input type='checkbox' id='equipos_" + itemIndex + "__Candado' name='equipos[" + itemIndex + "].Candado'/></td></tr>");
        //$("#myTable").append(newItem);
        
        $.getJSON("/ISS/Cargar_DB_MyTE", {},
            function (concData) {

                var optionsMarca = "<option selected disabled>Seleccione Marca</option>";
                var optionsTipoEquipo = "<option selected disabled>Seleccione Tipo</option>";

                for (var i = 0; i < concData.length; i++) {
                    if (concData[i].Value == 'marca')
                    {
                        //if (concData[i].Text == 'HP')
                        //    optionsMarca = optionsMarca + "<option selected value='" + concData[i].Text + "'>" + concData[i].Text + "</option>";
                        //else
                            optionsMarca = optionsMarca + "<option value='" + concData[i].Text + "'>" + concData[i].Text + "</option>";
                    }
                    else if (concData[i].Value == 'tipo')
                    {
                        //if (concData[i].Text == 'CPU')
                        //    optionsTipoEquipo = optionsTipoEquipo + "<option selected value='" + concData[i].Text + "'>" + concData[i].Text + "</option>";
                        //else
                            optionsTipoEquipo = optionsTipoEquipo + "<option value='" + concData[i].Text + "'>" + concData[i].Text + "</option>";
                    }
                    else
                    {
                    }
                }
                
                var itemIndex = $("#myTable input.iHidden").length;
                e.preventDefault();
                //var newItem = $("<tr><td><input id='Interests_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='Interests[" + itemIndex + "].Id' /><input type='text' id='Interests_" + itemIndex + "__InterestText' name='Interests[" + itemIndex + "].InterestText'/></td><td><input type='checkbox' value='true'  id='Interests_" + itemIndex + "__IsExperienced' name='Interests[" + itemIndex + "].IsExperienced' /></tr>");
                //var newItem = $("<tr><td><input id='Equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='Equipo[" + itemIndex + "].Id' /><input type='text' id='Equipos_" + itemIndex + "__tiponombre' name='Equipo[" + itemIndex + "].tiponombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__marcanombre' name='Equipo[" + itemIndex + "].marcanombre'/><input type='text' id='Equipos_" + itemIndex + "__modelonombre' name='Equipo[" + itemIndex + "].modelonombre'/><input type='text' id='Equipos_" + itemIndex + "__numeroserie' name='Equipo[" + itemIndex + "].numeroserie'/></tr>");
                //var newItem = $("<tr><td><input id='Equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='Equipo[" + itemIndex + "].Id' /><input type='text' id='Equipos_" + itemIndex + "__tiponombre' name='Equipo[" + itemIndex + "].tiponombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__marcanombre' name='Equipo[" + itemIndex + "].marcanombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__modelonombre' name='Equipo[" + itemIndex + "].modelonombre'/></td><td><input type='text' id='Equipos_" + itemIndex + "__numeroserie' name='Equipo[" + itemIndex + "].numeroserie'/></td></tr>");
                //var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td>@Html.EditorFor(modelItem => equipo.MarcaNombre, new { htmlAttributes = new { @id = 'equipos_" + itemIndex + "__MarcaNombre' @name = 'equipos[" + itemIndex + "].MarcaNombre' } })</td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td></tr>");
                //var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__MarcaNombre' name='equipos[" + itemIndex + "].MarcaNombre' /></td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td></tr>");
                //var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><input type='text' id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__MarcaNombre' name='equipos[" + itemIndex + "].MarcaNombre' /></td><td><input type='text' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre'/></td><td><input type='text' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie'/></td><td><input type='checkbox' value='false' id='equipos_" + itemIndex + "__Bolso' name='equipos[" + itemIndex + "].Bolso'/></td><td><input type='checkbox' value='false' id='equipos_" + itemIndex + "__Mouse' name='equipos[" + itemIndex + "].Mouse'/></td><td><input type='checkbox' id='equipos_" + itemIndex + "__Candado' name='equipos[" + itemIndex + "].Candado'/></td></tr>");
                var newItem = $("<tr><td><input id='equipos_" + itemIndex + "__Id' type='hidden' value='' class='iHidden'  name='equipos[" + itemIndex + "].Id' /><select id='equipos_" + itemIndex + "__TipoNombre' name='equipos[" + itemIndex + "].TipoNombre'>" + optionsTipoEquipo + "</select></td><td><select id='equipos_" + itemIndex + "__MarcaNombre' name='equipos[" + itemIndex + "].MarcaNombre'>" + optionsMarca + "</select></td><td><input type='text' class='uppercase' size='15' id='equipos_" + itemIndex + "__ModeloNombre' name='equipos[" + itemIndex + "].ModeloNombre' onkeypress='HandleKeyPress(event)' /></td><td><input type='text' class='uppercase' size='15' id='equipos_" + itemIndex + "__NumeroSerie' name='equipos[" + itemIndex + "].NumeroSerie' onkeypress='HandleKeyPress(event)' /></td><td style='text- align: center'><input type='checkbox' value='true' id='equipos_" + itemIndex + "__Bolso' name='equipos[" + itemIndex + "].Bolso'/></td><td style='text- align: center'><input type='checkbox' value='true' id='equipos_" + itemIndex + "__Mouse' name='equipos[" + itemIndex + "].Mouse'/></td><td style='text- align: center'><input type='checkbox' value='true' id='equipos_" + itemIndex + "__Candado' name='equipos[" + itemIndex + "].Candado'/></td><td><input type='text' size='10' id='equipos_" + itemIndex + "__Comentario' name='equipos[" + itemIndex + "].Comentario' onkeypress='HandleKeyPress(event)' /></td></tr>");
                $("#myTable").append(newItem); 

                //// no guardar guía (click automático en botón) al incorporar modelo, nro. serie o comentario
                //document.getElementById('equipos_' + itemIndex + '__ModeloNombre').addEventListener('keypress', function (event1) {
                //    if (event1.keyCode == 13) {
                //        event1.preventDefault();
                //    }
                //});

                //document.getElementById('equipos_' + itemIndex + '__NumeroSerie').addEventListener('keypress', function (event2) {
                //    if (event2.keyCode == 13) {
                //        event2.preventDefault();
                //    }
                //});

                //document.getElementById('equipos_' + itemIndex + '__Comentario').addEventListener('keypress', function (event3) {
                //    if (event3.keyCode == 13) {
                //        event3.preventDefault();
                //    }
                //});
            });
 

    });


    //$(document).ready(function () {
    //    $("#emailgerenteid").on('input', function () {
    //        $('#nombregerenteid').val($(this).val());
    //    });
    //});


    //$(document).ready(function () {
    //    $("#emailgerenteid").keyup(function () {
    //        $("#nombregerenteid").val($(this).val());
    //    });
    //});


    //$(document).ready(function () {
    //    $("#emailgerenteid").blur(function () {
    //        $("#nombregerenteid").val($(this).val());
    //    });
    //});



    $('.focus_ext :input').focus();



    if (document.getElementById("radiopendienteid") != null) {
        if (document.getElementById("radiopendienteid").checked == true) {
            limpiarEmailTecnicoEntId("radiopendienteid");
        }
    }

    if (document.getElementById("radioentregaid") != null) {
        if (document.getElementById("radioentregaid").checked == true) {
            limpiarEmailTecnicoEntId("radioentregaid");
        }
    }

    if (document.getElementById("radioretiraid") != null) {
        if (document.getElementById("radioretiraid").checked == true) {
            limpiarEmailTecnicoEntId("radioretiraid");
        }
    }

    if (document.getElementById("checktecnicopreid") != null) {
        if (document.getElementById("checktecnicopreid").checked == true) {
            limpiarEmailTecnicoPreId();
        }
    }
    


    



});






function HandleKeyPress(evt) {
    
    // no guardar guía (click automático en botón) al incorporar modelo, nro. serie o comentario

    var key = evt.which || evt.charCode || evt.keyCode || evt.key || 0;

    if (key == 13) {
        evt.preventDefault();
        }

}


function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (re.test(email)) {
        //document.getElementById(elemento).style.background = '#ff0000'; // fondo rojo, si esta correcto el email
        //document.getElementById(elemento + '_error1').style.display = "none";
        //document.getElementById(elemento + '_error2').style.display = "none";
        return true;
    } else {
        //document.getElementById(elemento).style.background = '#0000ff'; // fondo azul, si es error
        return false;
    }
}


function validateFormCrearGuia() {

    // Set error catcher
    var error = 0;

    if (document.getElementById("proveedorid") == null) {
    }
    else {
        if (document.getElementById('proveedorid').value.length == 0) {
            document.getElementById('proveedorid_error').style.display = "block";
            document.getElementById('proveedorid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('proveedorid_error').style.display = "none";
        }
    }

    if (document.getElementById("fechaguiaid") == null) {
    }
    else {
        if (document.getElementById('fechaguiaid').value.length == 0) {
            document.getElementById('fechaguiaid_error').style.display = "block";
            document.getElementById('fechaguiaid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('fechaguiaid_error').style.display = "none";
        }
    }

    if (document.getElementById("numeroguiaid") == null) {
        // nada por ahora
    }
    else {
        if (document.getElementById('numeroguiaid').value.length == 0) {
            document.getElementById('numeroguiaid_error').style.display = "block";
            document.getElementById('numeroguiaid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('numeroguiaid_error').style.display = "none";
        }
    }

    if (document.getElementById("ordencompraid") == null) {
        // nada por ahora
    }
    else {
        if (document.getElementById('ordencompraid').value.length == 0) {
            document.getElementById('ordencompraid_error').style.display = "block";
            document.getElementById('ordencompraid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('ordencompraid_error').style.display = "none";
        }
    }


    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateFormEquiposSinTecnico() {

    // Set error catcher
    var error = 0;

    if (document.getElementById("emailtecnicopreid") == null) {
    }
    else {

        if (document.getElementById('checktecnicopreid').checked == true)
        {
            // Limpiar
            $('#emailtecnicopreid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
            // Dejar pasar
        }
        else if (document.getElementById('emailtecnicopreid').value.length == 0) {
            document.getElementById('emailtecnicopreid_error2').style.display = "none";

            document.getElementById('emailtecnicopreid_error1').style.display = "block";
            document.getElementById('emailtecnicopreid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('emailtecnicopreid_error1').style.display = "none";

            if (!validateEmail(document.getElementById('emailtecnicopreid').value)) {
                document.getElementById('emailtecnicopreid_error2').style.display = "block";
                document.getElementById('emailtecnicopreid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicopreid_error2').style.display = "none";
            }
        }
        
    }
    
    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateFormEquiposIngresados() {

    // Set error catcher
    var error = 0;


    // Campos obligatorios

    if (document.getElementById("macroempresaid") == null) {
    }
    else {
        if (document.getElementById('macroempresaid').value.length == 0) {
            document.getElementById('macroempresaid_error').style.display = "block";
            document.getElementById('macroempresaid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('macroempresaid_error').style.display = "none";
        }
    }

    if (document.getElementById("empresarazonsocialid") == null) {
    }
    else {
        if (document.getElementById('empresarazonsocialid').value.length == 0) {
            document.getElementById('empresarazonsocialid_error').style.display = "block";
            document.getElementById('empresarazonsocialid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('empresarazonsocialid_error').style.display = "none";
        }
    }
    
    if (document.getElementById("centrocostoid") == null) {
    }
    else {
        if (document.getElementById('centrocostoid').value.length == 0) {
            document.getElementById('centrocostoid_error').style.display = "block";
            document.getElementById('centrocostoid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('centrocostoid_error').style.display = "none";
        }
    }

    if (document.getElementById("ubicacionid") == null) {
    }
    else {
        if (document.getElementById('ubicacionid').value.length == 0) {
            document.getElementById('ubicacionid_error').style.display = "block";
            document.getElementById('ubicacionid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('ubicacionid_error').style.display = "none";
        }
    }

    if (document.getElementById("proyectoid") == null) {
    }
    else {
        if (document.getElementById('proyectoid').value.length == 0) {
            document.getElementById('proyectoid_error').style.display = "block";
            document.getElementById('proyectoid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('proyectoid_error').style.display = "none";
        }
    }

    if (document.getElementById("comunaid") == null) {
    }
    else {
        if (document.getElementById('comunaid').value.length == 0) {
            document.getElementById('comunaid_error').style.display = "block";
            document.getElementById('comunaid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('comunaid_error').style.display = "none";
        }
    }

    if (document.getElementById("direccionid") == null) {
    }
    else {
        if (document.getElementById('direccionid').value.length == 0) {
            document.getElementById('direccionid_error').style.display = "block";
            document.getElementById('direccionid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('direccionid_error').style.display = "none";
        }
    }

    if (document.getElementById("emailgerenteid") == null) {
    }
    else {
        if (document.getElementById('emailgerenteid').value.length == 0) {
            document.getElementById('emailgerenteid_error2').style.display = "none";

            document.getElementById('emailgerenteid_error1').style.display = "block";
            document.getElementById('emailgerenteid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('emailgerenteid_error1').style.display = "none";

            if (!validateEmail(document.getElementById('emailgerenteid').value)) {
                document.getElementById('emailgerenteid_error2').style.display = "block";
                document.getElementById('emailgerenteid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailgerenteid_error2').style.display = "none";
            }
        }
    }


    // Validar emails
    if (document.getElementById("solicitanteid") == null) {
    }
    else {
        if (document.getElementById('solicitanteid').value.length > 0) {
            if (!validateEmail(document.getElementById('solicitanteid').value)) {
                document.getElementById('solicitanteid_error2').style.display = "block";
                document.getElementById('solicitanteid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('solicitanteid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('solicitanteid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailusuariofinalid") == null) {
    }
    else {
        if (document.getElementById('emailusuariofinalid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailusuariofinalid').value)) {
                document.getElementById('emailusuariofinalid_error2').style.display = "block";
                document.getElementById('emailusuariofinalid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailusuariofinalid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailusuariofinalid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailtecnicopreid") == null) {
    }
    else {
        if (document.getElementById('checktecnicopreid').checked == true) {
            // Limpiar
            $('#emailtecnicopreid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
            // Dejar pasar
        }
        else if (document.getElementById('emailtecnicopreid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailtecnicopreid').value)) {
                document.getElementById('emailtecnicopreid_error2').style.display = "block";
                document.getElementById('emailtecnicopreid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicopreid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
        }
    }



    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateFormModificarEquipo()
{
    // Set error catcher
    var error = 0;


    // Campos obligatorios del proyecto (DB: datos básicos)

    if (document.getElementById("macroempresaid") == null) {
    }
    else {
        if (document.getElementById('macroempresaid').value.length == 0) {
            document.getElementById('macroempresaid_error').style.display = "block";
            document.getElementById('macroempresaid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('macroempresaid_error').style.display = "none";
        }
    }

    if (document.getElementById("empresarazonsocialid") == null) {
    }
    else {
        if (document.getElementById('empresarazonsocialid').value.length == 0) {
            document.getElementById('empresarazonsocialid_error').style.display = "block";
            document.getElementById('empresarazonsocialid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('empresarazonsocialid_error').style.display = "none";
        }
    }

    if (document.getElementById("centrocostoid") == null) {
    }
    else {
        if (document.getElementById('centrocostoid').value.length == 0) {
            document.getElementById('centrocostoid_error').style.display = "block";
            document.getElementById('centrocostoid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('centrocostoid_error').style.display = "none";
        }
    }

    if (document.getElementById("ubicacionid") == null) {
    }
    else {
        if (document.getElementById('ubicacionid').value.length == 0) {
            document.getElementById('ubicacionid_error').style.display = "block";
            document.getElementById('ubicacionid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('ubicacionid_error').style.display = "none";
        }
    }

    if (document.getElementById("proyectoid") == null) {
    }
    else {
        if (document.getElementById('proyectoid').value.length == 0) {
            document.getElementById('proyectoid_error').style.display = "block";
            document.getElementById('proyectoid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('proyectoid_error').style.display = "none";
        }
    }

    if (document.getElementById("comunaid") == null) {
    }
    else {
        if (document.getElementById('comunaid').value.length == 0) {
            document.getElementById('comunaid_error').style.display = "block";
            document.getElementById('comunaid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('comunaid_error').style.display = "none";
        }
    }

    if (document.getElementById("direccionid") == null) {
    }
    else {
        if (document.getElementById('direccionid').value.length == 0) {
            document.getElementById('direccionid_error').style.display = "block";
            document.getElementById('direccionid_error').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('direccionid_error').style.display = "none";
        }
    }

    if (document.getElementById("emailgerenteid") == null) {
    }
    else {
        if (document.getElementById('emailgerenteid').value.length == 0) {
            document.getElementById('emailgerenteid_error2').style.display = "none";

            document.getElementById('emailgerenteid_error1').style.display = "block";
            document.getElementById('emailgerenteid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('emailgerenteid_error1').style.display = "none";

            if (!validateEmail(document.getElementById('emailgerenteid').value)) {
                document.getElementById('emailgerenteid_error2').style.display = "block";
                document.getElementById('emailgerenteid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailgerenteid_error2').style.display = "none";
            }
        }
    }


    // Validar emails

    if (document.getElementById("solicitanteid") == null) {
    }
    else {
        if (document.getElementById('solicitanteid').value.length > 0) {
            if (!validateEmail(document.getElementById('solicitanteid').value)) {
                document.getElementById('solicitanteid_error2').style.display = "block";
                document.getElementById('solicitanteid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('solicitanteid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('solicitanteid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailusuariofinalid") == null) {
    }
    else {
        if (document.getElementById('emailusuariofinalid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailusuariofinalid').value)) {
                document.getElementById('emailusuariofinalid_error2').style.display = "block";
                document.getElementById('emailusuariofinalid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailusuariofinalid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailusuariofinalid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailtecnicopreid") == null) {
    }
    else {
        if (document.getElementById('checktecnicopreid').checked == true) {
            // Limpiar
            $('#emailtecnicopreid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
            // Dejar pasar
        }
        else if (document.getElementById('emailtecnicopreid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailtecnicopreid').value)) {
                document.getElementById('emailtecnicopreid_error2').style.display = "block";
                document.getElementById('emailtecnicopreid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicopreid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailtecnicoentid") == null) {
    }
    else {
        if (document.getElementById('emailtecnicoentid').checked == true) {
            // Limpiar
            $('#emailtecnicoentid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicoentid_error2').style.display = "none";
            // Dejar pasar
        }
        else if (document.getElementById('emailtecnicoentid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailtecnicoentid').value)) {
                document.getElementById('emailtecnicoentid_error2').style.display = "block";
                document.getElementById('emailtecnicoentid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicoentid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailtecnicoentid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailquienrecibeid") == null) {
    }
    else {
        if (document.getElementById('emailquienrecibeid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailquienrecibeid').value)) {
                document.getElementById('emailquienrecibeid_error2').style.display = "block";
                document.getElementById('emailquienrecibeid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailquienrecibeid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailquienrecibeid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailquienretiraid") == null) {
    }
    else {
        if (document.getElementById('emailquienretiraid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailquienretiraid').value)) {
                document.getElementById('emailquienretiraid_error2').style.display = "block";
                document.getElementById('emailquienretiraid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailquienretiraid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailquienretiraid_error2').style.display = "none";
        }
    }


    // Para incorporar check Listo para Entrega

    if (document.getElementById("listoparaentregaid") == null) {
    }
    else {
        
        if (document.getElementById('listoparaentregaid').checked == true) {

            // Desea pasar de 4 (Equipos en Preparación) a 7 (Equipos Listos para Entrega)
            
            error = error + validarParte1();
        }
    }


    error = error + validarParte2_campos();


    // Para incorporar check 'Equipo Entregado' o 'Equipo Retirado'

    if ((document.getElementById("equipoentregadoid") != null) || (document.getElementById("equiporetiradoid") != null)) {

        if ((document.getElementById('equipoentregadoid').checked == true) || (document.getElementById('equiporetiradoid').checked == true)) {

            // Valida campos de Entrega y Retira
            error = error + validarParte2_estados();
        }
    }

    

    //// Para incorporar check Equipo Entregado (Ya NO existe este check en el formulario) 

    //if (document.getElementById("equipoentregadoid") == null) {
    //}
    //else {
    //    if (document.getElementById('equipoentregadoid').checked == true) {


    //        // Validar check Listo para Entrega

    //        if (document.getElementById("listoparaentregaid") == null) {
    //        }
    //        else {
    //            if (document.getElementById('listoparaentregaid').checked == true) {
    //                // Limpiar
    //                document.getElementById('listoparaentregaid_error1').style.display = "none";
    //                document.getElementById('listoparaentregaid_error3').style.display = "none";
    //                // Dejar pasar
    //            }
    //            else {
    //                document.getElementById('listoparaentregaid_error1').style.display = "none";

    //                document.getElementById('listoparaentregaid_error3').style.display = "block";
    //                document.getElementById('listoparaentregaid_error3').style.color = '#ff3300';
    //                error++;
    //            }
    //        }


    //        // Validar Distribución (Entrega o Retiro)
            
    //        if ((document.getElementById("radioentregaid") == null) && (document.getElementById("radioretiraid") == null)) {
    //        }
    //        else {

    //            if (document.getElementById("radioentregaid").checked == true)
    //            {
    //                // Limpiar Radio
    //                document.getElementById('radiodistribucionid_error1').style.display = "none";
    //                document.getElementById('radiodistribucionid_error3').style.display = "none";

    //                // Limpiar Retiro
    //                document.getElementById('fecharetiroid_error1').style.display = "none";
    //                document.getElementById('fecharetiroid_error3').style.display = "none";

    //                document.getElementById('emailquienretiraid_error1').style.display = "none";
    //                document.getElementById('emailquienretiraid_error2').style.display = "none";
    //                document.getElementById('emailquienretiraid_error3').style.display = "none";

    //                document.getElementById('nombrequienretiraid_error1').style.display = "none";
    //                document.getElementById('nombrequienretiraid_error3').style.display = "none";

    //                document.getElementById('rutquienretiraid_error1').style.display = "none";
    //                document.getElementById('rutquienretiraid_error3').style.display = "none";
                    

    //                if (document.getElementById('emailtecnicoentid').value.length == 0) {
    //                    document.getElementById('emailtecnicoentid_error1').style.display = "none";
    //                    document.getElementById('emailtecnicoentid_error2').style.display = "none";

    //                    document.getElementById('emailtecnicoentid_error3').style.display = "block";
    //                    document.getElementById('emailtecnicoentid_error3').style.color = '#ff3300';
    //                    error++;
    //                }
    //                else {
    //                    document.getElementById('emailtecnicoentid_error1').style.display = "none";
    //                    document.getElementById('emailtecnicoentid_error3').style.display = "none";

    //                    if (!validateEmail(document.getElementById('emailtecnicoentid').value)) {
    //                        document.getElementById('emailtecnicoentid_error2').style.display = "block";
    //                        document.getElementById('emailtecnicoentid_error2').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //                    }
    //                }

    //                if (document.getElementById("fechaentregaid") == null) {
    //                }
    //                else {
    //                    if (document.getElementById('fechaentregaid').value.length == 0) {
    //                        document.getElementById('fechaentregaid_error3').style.display = "block";
    //                        document.getElementById('fechaentregaid_error3').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('fechaentregaid_error3').style.display = "none";
    //                    }
    //                }

    //                if (document.getElementById('emailquienrecibeid').value.length == 0) {
    //                    document.getElementById('emailquienrecibeid_error1').style.display = "none";
    //                    document.getElementById('emailquienrecibeid_error2').style.display = "none";

    //                    document.getElementById('emailquienrecibeid_error3').style.display = "block";
    //                    document.getElementById('emailquienrecibeid_error3').style.color = '#ff3300';
    //                    error++;
    //                }
    //                else {
    //                    document.getElementById('emailquienrecibeid_error1').style.display = "none";
    //                    document.getElementById('emailquienrecibeid_error3').style.display = "none";

    //                    if (!validateEmail(document.getElementById('emailquienrecibeid').value)) {
    //                        document.getElementById('emailquienrecibeid_error2').style.display = "block";
    //                        document.getElementById('emailquienrecibeid_error2').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('emailquienrecibeid_error2').style.display = "none";
    //                    }
    //                }


    //            }
    //            else if (document.getElementById('radioretiraid').checked == true)
    //            {
    //                // Limpiar Radio
    //                document.getElementById('radiodistribucionid_error1').style.display = "none";
    //                document.getElementById('radiodistribucionid_error3').style.display = "none";

    //                // Limpiar Entrega
    //                document.getElementById('emailtecnicoentid_error1').style.display = "none";
    //                document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //                document.getElementById('emailtecnicoentid_error3').style.display = "none";

    //                document.getElementById('fechaentregaid_error1').style.display = "none";
    //                document.getElementById('fechaentregaid_error3').style.display = "none";

    //                document.getElementById('emailquienrecibeid_error1').style.display = "none";
    //                document.getElementById('emailquienrecibeid_error2').style.display = "none";
    //                document.getElementById('emailquienrecibeid_error3').style.display = "none";
                    

    //                if (document.getElementById("fecharetiroid") == null) {
    //                }
    //                else {
    //                    if (document.getElementById('fecharetiroid').value.length == 0) {
    //                        document.getElementById('fecharetiroid_error3').style.display = "block";
    //                        document.getElementById('fecharetiroid_error3').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('fecharetiroid_error3').style.display = "none";
    //                    }
    //                }

    //                if (document.getElementById('emailquienretiraid').value.length == 0) {
    //                    document.getElementById('emailquienretiraid_error1').style.display = "none";
    //                    document.getElementById('emailquienretiraid_error2').style.display = "none";

    //                    document.getElementById('emailquienretiraid_error3').style.display = "block";
    //                    document.getElementById('emailquienretiraid_error3').style.color = '#ff3300';
    //                    error++;
    //                }
    //                else {
    //                    document.getElementById('emailquienretiraid_error1').style.display = "none";
    //                    document.getElementById('emailquienretiraid_error3').style.display = "none";

    //                    if (!validateEmail(document.getElementById('emailquienretiraid').value)) {
    //                        document.getElementById('emailquienretiraid_error2').style.display = "block";
    //                        document.getElementById('emailquienretiraid_error2').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('emailquienretiraid_error2').style.display = "none";
    //                    }
    //                }

    //                if (document.getElementById("nombrequienretiraid") == null) {
    //                }
    //                else {
    //                    if (document.getElementById('nombrequienretiraid').value.length == 0) {
    //                        document.getElementById('nombrequienretiraid_error3').style.display = "block";
    //                        document.getElementById('nombrequienretiraid_error3').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('nombrequienretiraid_error3').style.display = "none";
    //                    }
    //                }

    //                if (document.getElementById("rutquienretiraid") == null) {
    //                }
    //                else {
    //                    if (document.getElementById('rutquienretiraid').value.length == 0) {
    //                        document.getElementById('rutquienretiraid_error3').style.display = "block";
    //                        document.getElementById('rutquienretiraid_error3').style.color = '#ff3300';
    //                        error++;
    //                    }
    //                    else {
    //                        document.getElementById('rutquienretiraid_error3').style.display = "none";
    //                    }
    //                }
                    

    //            }
    //            else
    //            {
    //                document.getElementById('radiodistribucionid_error3').style.display = "block";
    //                document.getElementById('radiodistribucionid_error3').style.color = '#ff3300';
    //                error++;
    //            }
                
    //        }

    //    }
    //    else {
    //        document.getElementById('listoparaentregaid_error1').style.display = "none";
    //        document.getElementById('listoparaentregaid_error3').style.display = "none";

    //        document.getElementById('radiodistribucionid_error1').style.display = "none";
    //        document.getElementById('radiodistribucionid_error3').style.display = "none";
    //    }
    //}


    // Don't submit form if there are errors
    if (error > 0) {
        //alert("Favor incorporar e-mail de Usuario Final y/o Técnico (o check 'No requiere técnico) antes de Entregar");
        return false;
    }
}


function validateFormEquiposListosParaEntrega() {
    // Set error catcher
    var error = 0;


    // Valida campos de Entrega y Retira
    error = error + validarParte2_campos();

    if ((document.getElementById("equipoentregadoid") != null) || (document.getElementById("equiporetiradoid") != null)) {

        if ((document.getElementById('equipoentregadoid').checked == true) || (document.getElementById('equiporetiradoid').checked == true)) {

            // Valida campos de Entrega y Retira
            error = error + validarParte2_estados();
        }
    }


    //if (document.getElementById("emailtecnicoentid") == null) {
    //}
    //else {

    //    if (document.getElementById('radioretiraid').checked == true) {
    //        // Limpiar
    //        $('#emailtecnicoentid').typeahead('val', null).trigger('keyup');
    //        document.getElementById('emailtecnicoentid_error1').style.display = "none";
    //        document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //        document.getElementById('emailtecnicoentid_error3').style.display = "none";
    //        // Dejar pasar
    //    }
    //    else if (document.getElementById('emailtecnicoentid').value.length == 0) {
    //        document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //        document.getElementById('emailtecnicoentid_error3').style.display = "none";

    //        document.getElementById('emailtecnicoentid_error1').style.display = "block";
    //        document.getElementById('emailtecnicoentid_error1').style.color = '#ff3300';

    //        error++;
    //    }
    //    else {
    //        document.getElementById('emailtecnicoentid_error1').style.display = "none";

    //        if (!validateEmail(document.getElementById('emailtecnicoentid').value)) {
    //            document.getElementById('emailtecnicoentid_error2').style.display = "block";
    //            document.getElementById('emailtecnicoentid_error2').style.color = '#ff3300';
    //            error++;
    //        }
    //        else {
    //            document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //        }
    //    }

    //}



    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }
}



function validateFormEquiposPreparados() {
    // Set error catcher
    var error = 0;


    // Valida campos de Entrega y Retira
    error = error + validarParte2_campos();

    if ((document.getElementById("equipoentregadoid") != null) || (document.getElementById("equiporetiradoid") != null)) {

        if ((document.getElementById('equipoentregadoid').checked == true) || (document.getElementById('equiporetiradoid').checked == true)) {

            // Valida campos de Entrega y Retira
            //error = error + validarParte2_estados();
        }
    }


    //if (document.getElementById("emailtecnicoentid") == null) {
    //}
    //else {

    //    if (document.getElementById('radioretiraid').checked == true) {
    //        // Limpiar
    //        $('#emailtecnicoentid').typeahead('val', null).trigger('keyup');
    //        document.getElementById('emailtecnicoentid_error1').style.display = "none";
    //        document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //        document.getElementById('emailtecnicoentid_error3').style.display = "none";
    //        // Dejar pasar
    //    }
    //    else if (document.getElementById('emailtecnicoentid').value.length == 0) {
    //        document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //        document.getElementById('emailtecnicoentid_error3').style.display = "none";

    //        document.getElementById('emailtecnicoentid_error1').style.display = "block";
    //        document.getElementById('emailtecnicoentid_error1').style.color = '#ff3300';

    //        error++;
    //    }
    //    else {
    //        document.getElementById('emailtecnicoentid_error1').style.display = "none";

    //        if (!validateEmail(document.getElementById('emailtecnicoentid').value)) {
    //            document.getElementById('emailtecnicoentid_error2').style.display = "block";
    //            document.getElementById('emailtecnicoentid_error2').style.color = '#ff3300';
    //            error++;
    //        }
    //        else {
    //            document.getElementById('emailtecnicoentid_error2').style.display = "none";
    //        }
    //    }

    //}



    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }
}




function validateEquiposConDatosBasicos() {

    // Set error catcher
    var error = 0;


    // Validar emails

    if (document.getElementById("solicitanteid") == null) {
    }
    else {
        if (document.getElementById('solicitanteid').value.length > 0) {
            if (!validateEmail(document.getElementById('solicitanteid').value)) {
                document.getElementById('solicitanteid_error2').style.display = "block";
                document.getElementById('solicitanteid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('solicitanteid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('solicitanteid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailusuariofinalid") == null) {
    }
    else {
        if (document.getElementById('emailusuariofinalid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailusuariofinalid').value)) {
                document.getElementById('emailusuariofinalid_error2').style.display = "block";
                document.getElementById('emailusuariofinalid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailusuariofinalid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailusuariofinalid_error2').style.display = "none";
        }
    }

    if (document.getElementById("emailtecnicopreid") == null) {
    }
    else {
        if (document.getElementById('checktecnicopreid').checked == true) {
            // Limpiar
            $('#emailtecnicopreid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
            // Dejar pasar
        }
        else if (document.getElementById('emailtecnicopreid').value.length > 0) {
            if (!validateEmail(document.getElementById('emailtecnicopreid').value)) {
                document.getElementById('emailtecnicopreid_error2').style.display = "block";
                document.getElementById('emailtecnicopreid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicopreid_error2').style.display = "none";
            }
        }
        else {
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
        }
    }
    


    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateEquiposEnPreparacion() {

    // Set error catcher
    var error = 0;

    if (document.getElementById("emailusuariofinalid") == null) {
    }
    else {
        if (document.getElementById('emailusuariofinalid').value.length == 0) {
            document.getElementById('emailusuariofinalid_error2').style.display = "none";

            document.getElementById('emailusuariofinalid_error1').style.display = "block";
            document.getElementById('emailusuariofinalid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('emailusuariofinalid_error1').style.display = "none";

            if (!validateEmail(document.getElementById('emailusuariofinalid').value)) {
                document.getElementById('emailusuariofinalid_error2').style.display = "block";
                document.getElementById('emailusuariofinalid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailusuariofinalid_error2').style.display = "none";
            }
        }
    }

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateEquiposSinUsuarioFinal() {

    // Set error catcher
    var error = 0;

    if (document.getElementById("emailusuariofinalid") == null) {
    }
    else {
        if (document.getElementById('emailusuariofinalid').value.length == 0) {
            document.getElementById('emailusuariofinalid_error2').style.display = "none";

            document.getElementById('emailusuariofinalid_error1').style.display = "block";
            document.getElementById('emailusuariofinalid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('emailusuariofinalid_error1').style.display = "none";

            if (!validateEmail(document.getElementById('emailusuariofinalid').value)) {
                document.getElementById('emailusuariofinalid_error2').style.display = "block";
                document.getElementById('emailusuariofinalid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailusuariofinalid_error2').style.display = "none";
            }
        }
    }

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}



function limpiarEmailTecnicoPreId() {

    if (document.getElementById('checktecnicopreid').checked == true) {
        // Limpiar
        $('#emailtecnicopreid').typeahead('val', null).trigger('keyup');
        document.getElementById('emailtecnicopreid_error1').style.display = "none";
        document.getElementById('emailtecnicopreid_error2').style.display = "none";
        document.getElementById('emailtecnicopreid_error3').style.display = "none";

        // Deshabilitar
        $('#emailtecnicopreid').attr("disabled", true);
        
        if (document.getElementById('listoparaentregaid') != null) {
            document.getElementById('listoparaentregaid_error1').style.display = "none";
            document.getElementById('listoparaentregaid_error3').style.display = "none";

            document.getElementById('listoparaentregaid').checked = false;
            $('#listoparaentregaid').attr("disabled", true);
        }
    }
    else
    {
        $('#emailtecnicopreid').attr("disabled", false);

        if (document.getElementById('listoparaentregaid') != null) {
            $('#listoparaentregaid').attr("disabled", false);
        }
    }

} 



function limpiarEmailTecnicoEntId(tipo) {

    // tipo.id se ejecuta desde el onclick
    // tipo se ejecuta desde formularios como "Eq. para Retiro" o "Modificar Equipo"

    var aux_cap = 0;

    if ((tipo.id == "radioretiraid") || (tipo == "radioretiraid"))
    {
        // Retiro (habilitar)

        //var aux1 = tipo.id;
        //var aux2 = tipo;

        if (document.getElementById('fecharetiroid') != null)
        {
            $('#fecharetiroid').attr("disabled", false);
            
            var myValFR2100 = $('#fecharetiroid').val();

            if (myValFR2100 != null && myValFR2100 != '') {

            }
            else {
                // Cargar campo automáticamente
                if (tipo.id == "radioretiraid")
                    $('#fecharetiroid').datepicker('setDate', new Date()).trigger('keyup');
            }
        }

        if (document.getElementById('emailquienretiraid') != null)
        {   
            $('#emailquienretiraid').attr("disabled", false);

            var myValTE2112= $('#emailquienretiraid').val();

            if (myValTE2112 != null && myValTE2112 != '') {
                $('#divemailquienretira .typeahead').val(myValTE2112).trigger('keyup');
            }
            else {
                // Cargar campo automáticamente
                if (document.getElementById("emailusuariofinalid") != null) {
                    var myValTE2120 = $('#emailusuariofinalid').val();                                // toma el valor de Tecnico Pre. (email)
                    $('#divemailquienretira .typeahead').val(myValTE2120).trigger('keyup');
                }
            }
        }

        if (document.getElementById('nombrequienretiraid') != null)
        {
            $('#nombrequienretiraid').attr("disabled", false);
        }

        if (document.getElementById('rutquienretiraid') != null)
        {
            $('#rutquienretiraid').attr("disabled", false);
        }

        if (document.getElementById('equiporetiradoid') != null) {
            $('#equiporetiradoid').attr("disabled", false);
        }


        // Entrega (limpiar y deshabilitar)

        if (document.getElementById('emailtecnicoentid') != null) {
            // Limpiar
            $('#emailtecnicoentid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicoentid_error1').style.display = "none";
            document.getElementById('emailtecnicoentid_error2').style.display = "none";
            document.getElementById('emailtecnicoentid_error3').style.display = "none";

            // Deshabilitar
            $('#emailtecnicoentid').attr("disabled", true);
        }

        if (document.getElementById('fechaentregaid') != null) {
            // Limpiar
            $('#fechaentregaid').val(null).trigger('keyup');
            document.getElementById('fechaentregaid_error1').style.display = "none";
            document.getElementById('fechaentregaid_error3').style.display = "none";

            // Deshabilitar
            $('#fechaentregaid').attr("disabled", true);
        }

        if (document.getElementById('emailquienrecibeid') != null) {
            // Limpiar
            $('#emailquienrecibeid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailquienrecibeid_error1').style.display = "none";
            document.getElementById('emailquienrecibeid_error2').style.display = "none";
            document.getElementById('emailquienrecibeid_error3').style.display = "none";

            // Deshabilitar
            $('#emailquienrecibeid').attr("disabled", true);
        }

        if (document.getElementById('equipoentregadoid') != null) {
            // Deshabilitar
            document.getElementById('equipoentregadoid').checked = false;
            $('#equipoentregadoid').attr("disabled", true);
        }
    }
    else if ((tipo.id == "radioentregaid") || ((tipo == "radioentregaid")))
    {
        // Entrega (habilitar)

        if (document.getElementById('emailtecnicoentid') != null)
        {
            $('#emailtecnicoentid').attr("disabled", false);
            
            var myValTE2186 = $('#emailtecnicoentid').val();

            if (myValTE2186 != null && myValTE2186 != '') {
                $('#divemailtecnicoent .typeahead').val(myValTE2186).trigger('keyup');
            }
            else {
                // Cargar campo automáticamente
                if (document.getElementById("emailtecnicopreid") != null) {
                    var myValTE2191 = $('#emailtecnicopreid').val();                                // toma el valor de Tecnico Pre. (email)
                    $('#divemailtecnicoent .typeahead').val(myValTE2191).trigger('keyup');
                }
            }
        }
        
        if (document.getElementById('fechaentregaid') != null)
        {
            $('#fechaentregaid').attr("disabled", false);

            var myValFE2210 = $('#fechaentregaid').val();

            if (myValFE2210 != null && myValFE2210 != '') {

            }
            else {
                // Cargar campo automáticamente
                if (tipo.id == "radioentregaid")
                    $('#fechaentregaid').datepicker('setDate', new Date()).trigger('keyup');
            }
        }

        if (document.getElementById('emailquienrecibeid') != null)
        {
            $('#emailquienrecibeid').attr("disabled", false);

            var myValTE2208 = $('#emailquienrecibeid').val();

            if (myValTE2208 != null && myValTE2208 != '') {
                $('#divemailquienrecibe .typeahead').val(myValTE2208).trigger('keyup');
            }
            else {
                // Cargar campo automáticamente
                if (document.getElementById("emailusuariofinalid") != null) {
                    var myValTE2215 = $('#emailusuariofinalid').val();                                // toma el valor de Tecnico Pre. (email)
                    //$('#divemailquienrecibe .typeahead').val(myValTE2215).trigger('keyup');
                    //$('#emailquienrecibeid').typeahead('val', myValTE2215).trigger('keyup');
                    //$('#divemailquienrecibe .typeahead').typeahead('val', myValTE2215).trigger('keyup');
                    //$('#divemailquienrecibe .typeahead').typeahead('val', myValTE2215);
                    //$('#emailquienrecibeid').typeahead('val', myValTE2215);
                    //$('#emailquienrecibeid').val(myValTE2215).trigger('keyup');
                    //$("#divemailquienrecibe .typeahead").typeahead("val", myValTE2215).trigger('keyup');
                    $("#divemailquienrecibe .typeahead").val(myValTE2215).trigger('keyup');
                    

                    //var myValTE2319 = $('#emailquienrecibeid').val();
                    //$('#emailquienrecibeid').typeahead('val', myValTE2319).trigger('keyup');
                }
            }
        }

        if (document.getElementById('equipoentregadoid') != null) {
            $('#equipoentregadoid').attr("disabled", false);
        }


        // Retiro (limpiar y deshabilitar)

        if (document.getElementById('fecharetiroid') != null) {
            // Limpiar
            $('#fecharetiroid').val(null).trigger('keyup');
            document.getElementById('fecharetiroid_error1').style.display = "none";
            document.getElementById('fecharetiroid_error3').style.display = "none";

            // Deshabilitar
            $('#fecharetiroid').attr("disabled", true);
        }

        if (document.getElementById('emailquienretiraid') != null) {
            // Limpiar
            $('#emailquienretiraid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailquienretiraid_error1').style.display = "none";
            document.getElementById('emailquienretiraid_error2').style.display = "none";
            document.getElementById('emailquienretiraid_error3').style.display = "none";

            // Deshabilitar
            $('#emailquienretiraid').attr("disabled", true);
        }

        if (document.getElementById('nombrequienretiraid') != null) {
            // Limpiar
            $('#nombrequienretiraid').val(null).trigger('keyup');
            document.getElementById('nombrequienretiraid_error1').style.display = "none";
            document.getElementById('nombrequienretiraid_error3').style.display = "none";
            document.getElementById('nombrequienretiraid_error5').style.display = "none";

            // Deshabilitar
            $('#nombrequienretiraid').attr("disabled", true);
        }

        if (document.getElementById('rutquienretiraid') != null) {
            // Limpiar
            $('#rutquienretiraid').val(null).trigger('keyup');
            document.getElementById('rutquienretiraid_error1').style.display = "none";
            document.getElementById('rutquienretiraid_error3').style.display = "none";
            document.getElementById('rutquienretiraid_error4').style.display = "none";
            document.getElementById('rutquienretiraid_error5').style.display = "none";

            // Deshabilitar
            $('#rutquienretiraid').attr("disabled", true);
        }

        if (document.getElementById('equiporetiradoid') != null) {
            // Deshabilitar
            document.getElementById('equiporetiradoid').checked = false;
            $('#equiporetiradoid').attr("disabled", true);
        }
    }
    else if ((tipo.id == "radiopendienteid") || ((tipo == "radiopendienteid")))
    {
        // Entrega (limpiar y deshabilitar)

        if (document.getElementById('emailtecnicoentid') != null) {
            // Limpiar
            $('#emailtecnicoentid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicoentid_error1').style.display = "none";
            document.getElementById('emailtecnicoentid_error2').style.display = "none";
            document.getElementById('emailtecnicoentid_error3').style.display = "none";

            // Deshabilitar
            $('#emailtecnicoentid').attr("disabled", true);
        }

        if (document.getElementById('fechaentregaid') != null) {
            // Limpiar
            $('#fechaentregaid').val(null).trigger('keyup');
            document.getElementById('fechaentregaid_error1').style.display = "none";
            document.getElementById('fechaentregaid_error3').style.display = "none";

            // Deshabilitar
            $('#fechaentregaid').attr("disabled", true);
        }

        if (document.getElementById('emailquienrecibeid') != null) {
            // Limpiar
            $('#emailquienrecibeid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailquienrecibeid_error1').style.display = "none";
            document.getElementById('emailquienrecibeid_error2').style.display = "none";
            document.getElementById('emailquienrecibeid_error3').style.display = "none";

            // Deshabilitar
            $('#emailquienrecibeid').attr("disabled", true);
        }

        if (document.getElementById('equipoentregadoid') != null) {
            // Deshabilitar
            document.getElementById('equipoentregadoid').checked = false;
            $('#equipoentregadoid').attr("disabled", true);
        }
        

        // Retiro (limpiar y deshabilitar)

        if (document.getElementById('fecharetiroid') != null) {
            // Limpiar
            $('#fecharetiroid').val(null).trigger('keyup');
            document.getElementById('fecharetiroid_error1').style.display = "none";
            document.getElementById('fecharetiroid_error3').style.display = "none";

            // Deshabilitar
            $('#fecharetiroid').attr("disabled", true);
        }

        if (document.getElementById('emailquienretiraid') != null) {
            // Limpiar
            $('#emailquienretiraid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailquienretiraid_error1').style.display = "none";
            document.getElementById('emailquienretiraid_error2').style.display = "none";
            document.getElementById('emailquienretiraid_error3').style.display = "none";

            // Deshabilitar
            $('#emailquienretiraid').attr("disabled", true);
        }

        if (document.getElementById('nombrequienretiraid') != null) {
            // Limpiar
            $('#nombrequienretiraid').val(null).trigger('keyup');
            document.getElementById('nombrequienretiraid_error1').style.display = "none";
            document.getElementById('nombrequienretiraid_error3').style.display = "none";
            document.getElementById('nombrequienretiraid_error5').style.display = "none";

            // Deshabilitar
            $('#nombrequienretiraid').attr("disabled", true);
        }

        if (document.getElementById('rutquienretiraid') != null) {
            // Limpiar
            $('#rutquienretiraid').val(null).trigger('keyup');
            document.getElementById('rutquienretiraid_error1').style.display = "none";
            document.getElementById('rutquienretiraid_error3').style.display = "none";
            document.getElementById('rutquienretiraid_error4').style.display = "none";
            document.getElementById('rutquienretiraid_error5').style.display = "none";

            // Deshabilitar
            $('#rutquienretiraid').attr("disabled", true);
        }
        
        if (document.getElementById('equiporetiradoid') != null) {
            // Deshabilitar
            document.getElementById('equiporetiradoid').checked = false;
            $('#equiporetiradoid').attr("disabled", true);
        }
        
    }

    
}



function validarParte1()
{
    // Valida que exista UF y T (o NT)
    // No se considera DB ya que son campos obligatorios en formulario

    var error = 0;

    // Validar Usuario Final

    if (document.getElementById("emailusuariofinalid") == null) {
    }
    else {
        if (document.getElementById('emailusuariofinalid').value.length == 0) {
            document.getElementById('emailusuariofinalid_error2').style.display = "none";

            document.getElementById('emailusuariofinalid_error3').style.display = "block";
            document.getElementById('emailusuariofinalid_error3').style.color = '#ff3300';

            error++;
        }
        else {
            document.getElementById('emailusuariofinalid_error3').style.display = "none";

            if (!validateEmail(document.getElementById('emailusuariofinalid').value)) {
                document.getElementById('emailusuariofinalid_error2').style.display = "block";
                document.getElementById('emailusuariofinalid_error2').style.color = '#ff3300';

                error++;
            }
            else {
                document.getElementById('emailusuariofinalid_error2').style.display = "none";
                document.getElementById('emailusuariofinalid_error3').style.display = "none";
            }
        }
    }


    // Validar Técnico (o check 'No requiere técnico')

    if (document.getElementById("emailtecnicopreid") == null) {
    }
    else {
        if (document.getElementById('checktecnicopreid').checked == true) {
            // Limpiar
            $('#emailtecnicopreid').typeahead('val', null).trigger('keyup');
            document.getElementById('emailtecnicopreid_error2').style.display = "none";
            document.getElementById('emailtecnicopreid_error3').style.display = "none";
            // Dejar pasar
        }
        else if (document.getElementById('emailtecnicopreid').value.length == 0) {
            document.getElementById('emailtecnicopreid_error2').style.display = "none";

            document.getElementById('emailtecnicopreid_error3').style.display = "block";
            document.getElementById('emailtecnicopreid_error3').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('emailtecnicopreid_error3').style.display = "none";

            if (!validateEmail(document.getElementById('emailtecnicopreid').value)) {
                document.getElementById('emailtecnicopreid_error2').style.display = "block";
                document.getElementById('emailtecnicopreid_error2').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicopreid_error2').style.display = "none";
                document.getElementById('emailtecnicopreid_error3').style.display = "none";

                // Validar check 'Listo para Entrega'
                if (document.getElementById("listoparaentregaid") != null) {

                    if (document.getElementById('listoparaentregaid').checked == true) {
                        // Limpiar
                        document.getElementById('listoparaentregaid_error3').style.display = "none";
                        // Dejar pasar
                    }
                    else {
                        document.getElementById('listoparaentregaid_error3').style.display = "block";
                        document.getElementById('listoparaentregaid_error3').style.color = '#ff3300';
                        error++;
                    }

                }

            }
        }
    }

    return error;
}



var Fn = {
    // Valida el rut con su cadena completa "XXXXXXXX-X"
    validaRut: function (rutCompleto) {
        rutCompleto = rutCompleto.replace("‐", "-");
        if (!/^[0-9]+[-|‐]{1}[0-9kK]{1}$/.test(rutCompleto))
            return false;
        var tmp = rutCompleto.split('-');
        var digv = tmp[1];
        var rut = tmp[0];
        if (digv == 'K') digv = 'k';

        return (Fn.dv(rut) == digv);
    },
    dv: function (T) {
        var M = 0, S = 1;
        for (; T; T = Math.floor(T / 10))
            S = (S + T % 10 * (9 - M++ % 6)) % 11;
        return S ? S - 1 : 'k';
    }
}


function validarParte2_campos() {

    // Validar campos para Entrega o Retira

    var error = 0;


    // Para Entrega

    //error = error + validarRadioEntregaId();

    if (document.getElementById("radioentregaid") == null) {
    }
    else {
        if (document.getElementById("radioentregaid").checked == true) {

            // Limpiar Entrega
            document.getElementById('fechaentregaid_error1').style.display = "none";
            document.getElementById('fechaentregaid_error3').style.display = "none";

            document.getElementById('emailquienrecibeid_error1').style.display = "none";
            document.getElementById('emailquienrecibeid_error2').style.display = "none";
            document.getElementById('emailquienrecibeid_error3').style.display = "none";

            // Validar email de técnico que entrega
            if (document.getElementById('emailtecnicoentid').value.length == 0) {
                document.getElementById('emailtecnicoentid_error1').style.display = "none";
                document.getElementById('emailtecnicoentid_error2').style.display = "none";

                // 2018-11-08 se incorporar una linea y se comentan 3
                document.getElementById('emailtecnicoentid_error3').style.display = "none";
                //document.getElementById('emailtecnicoentid_error3').style.display = "block";
                //document.getElementById('emailtecnicoentid_error3').style.color = '#ff3300';
                //error++;
            }
            else {
                document.getElementById('emailtecnicoentid_error1').style.display = "none";
                document.getElementById('emailtecnicoentid_error3').style.display = "none";

                if (!validateEmail(document.getElementById('emailtecnicoentid').value)) {
                    document.getElementById('emailtecnicoentid_error2').style.display = "block";
                    document.getElementById('emailtecnicoentid_error2').style.color = '#ff3300';
                    error++;
                }
                else {
                    document.getElementById('emailtecnicoentid_error2').style.display = "none";
                }
            }


            // Validar email Quien Recibe
            if (document.getElementById('emailquienrecibeid').value.length == 0) {
                document.getElementById('emailquienrecibeid_error1').style.display = "none";
                document.getElementById('emailquienrecibeid_error2').style.display = "none";
                document.getElementById('emailquienrecibeid_error3').style.display = "none";
            }
            else {
                document.getElementById('emailquienrecibeid_error1').style.display = "none";
                document.getElementById('emailquienrecibeid_error3').style.display = "none";

                if (!validateEmail(document.getElementById('emailquienrecibeid').value)) {
                    document.getElementById('emailquienrecibeid_error2').style.display = "block";
                    document.getElementById('emailquienrecibeid_error2').style.color = '#ff3300';
                    error++;
                }
                else {
                    document.getElementById('emailquienrecibeid_error2').style.display = "none";
                }
            }


            // Validar que contenga DB, UF y T (o NT)
            error = error + validarParte1();

            // Limpiar campos de Retira
            $('#emailquienretiraid').typeahead('val', null).trigger('keyup');
            $('#fecharetiroid').val(null).trigger('keyup');
            $('#nombrequienretiraid').val(null).trigger('keyup');
            $('#rutquienretiraid').val(null).trigger('keyup');
        }
        else {
            document.getElementById('emailtecnicoentid_error1').style.display = "none";
            document.getElementById('emailtecnicoentid_error2').style.display = "none";
            document.getElementById('emailtecnicoentid_error3').style.display = "none";
        }
    }



    // Para Retiro

    if (document.getElementById("radioretiraid") == null) {
    }
    else {
        if (document.getElementById("radioretiraid").checked == true) {

            // Limpiar Retiro
            document.getElementById('fecharetiroid_error1').style.display = "none";
            document.getElementById('fecharetiroid_error3').style.display = "none";

            document.getElementById('emailquienretiraid_error1').style.display = "none";
            document.getElementById('emailquienretiraid_error2').style.display = "none";
            document.getElementById('emailquienretiraid_error3').style.display = "none";

            document.getElementById('nombrequienretiraid_error1').style.display = "none";
            document.getElementById('nombrequienretiraid_error3').style.display = "none";
            document.getElementById('nombrequienretiraid_error5').style.display = "none";

            document.getElementById('rutquienretiraid_error1').style.display = "none";
            document.getElementById('rutquienretiraid_error3').style.display = "none";
            document.getElementById('rutquienretiraid_error4').style.display = "none";
            document.getElementById('rutquienretiraid_error5').style.display = "none";
            
            // Validar que contenga DB, UF y T (o NT)
            error = error + validarParte1();

            // Limpiar campos de Entrega
            $('#emailtecnicoentid').typeahead('val', null).trigger('keyup');
            $('#emailquienrecibeid').typeahead('val', null).trigger('keyup');
            $('#fechaentregaid').val(null).trigger('keyup');



            // Validar email Quien Retira
            if (document.getElementById('emailquienretiraid').value.length == 0) {
                document.getElementById('emailquienretiraid_error1').style.display = "none";
                document.getElementById('emailquienretiraid_error2').style.display = "none";
                document.getElementById('emailquienretiraid_error3').style.display = "none";
            }
            else {
                document.getElementById('emailquienretiraid_error1').style.display = "none";
                document.getElementById('emailquienretiraid_error3').style.display = "none";

                if (!validateEmail(document.getElementById('emailquienretiraid').value)) {
                    document.getElementById('emailquienretiraid_error2').style.display = "block";
                    document.getElementById('emailquienretiraid_error2').style.color = '#ff3300';
                    error++;
                }
                else {
                    document.getElementById('emailquienretiraid_error2').style.display = "none";
                }
            }


            if ((document.getElementById("rutquienretiraid") != null) && (document.getElementById("nombrequienretiraid") != null)) {

                if ((document.getElementById('rutquienretiraid').value.length > 0) && (document.getElementById('nombrequienretiraid').value.length > 0)) {
                    // Dejar pasar
                    document.getElementById('rutquienretiraid_error5').style.display = "none";
                    document.getElementById('nombrequienretiraid_error5').style.display = "none";
                }
                else if (document.getElementById('rutquienretiraid').value.length > 0) {
                    document.getElementById('nombrequienretiraid_error5').style.display = "block";
                    document.getElementById('nombrequienretiraid_error5').style.color = '#ff3300';
                    error++;
                }
                else if (document.getElementById('nombrequienretiraid').value.length > 0) {
                    document.getElementById('rutquienretiraid_error5').style.display = "block";
                    document.getElementById('rutquienretiraid_error5').style.color = '#ff3300';
                    error++;
                }
                else {
                    document.getElementById('rutquienretiraid_error5').style.display = "none";
                    document.getElementById('nombrequienretiraid_error5').style.display = "none";
                }
            }


        }
        else {

        }
    }

    
    // Validar RUT

    if (document.getElementById("rutquienretiraid") != null) {

        if (document.getElementById('rutquienretiraid').value.length > 0) {

            if (Fn.validaRut($("#rutquienretiraid").val())) {
                document.getElementById('rutquienretiraid_error4').style.display = "none";
            }
            else {
                document.getElementById('rutquienretiraid_error4').style.display = "block";
                document.getElementById('rutquienretiraid_error4').style.color = '#ff3300';
                error++;
            }

        }
    }




    return error;


}


function validarParte2_estados() {

    // Para formulario ModificarEquipo
    // Valida que si se marca check de Equipo Entregado/Retirado, tengo los campos anteriores de Entrega (fecha y email) o Retiro (fecha, rut y email)

    var error = 0;


    // Para Equipo Entregado

    if (document.getElementById("equipoentregadoid") != null) {
        if (document.getElementById("equipoentregadoid").checked == true) {

            // Validar email de técnico que entrega
            if (document.getElementById('emailtecnicoentid').value.length == 0) {
                document.getElementById('emailtecnicoentid_error3').style.display = "block";
                document.getElementById('emailtecnicoentid_error3').style.color = '#ff3300';
                error++;
            }
            else {
                document.getElementById('emailtecnicoentid_error3').style.display = "none";
            }

            if ((document.getElementById("fechaentregaid") != null) && (document.getElementById("emailquienrecibeid") != null)) {

                if ((document.getElementById('fechaentregaid').value.length > 0) && (document.getElementById('emailquienrecibeid').value.length > 0)) {
                    // Dejar pasar
                    document.getElementById('fechaentregaid_error3').style.display = "none";
                    document.getElementById('emailquienrecibeid_error3').style.display = "none";
                }
                else if (document.getElementById('fechaentregaid').value.length > 0) {
                    document.getElementById('emailquienrecibeid_error3').style.display = "block";
                    document.getElementById('emailquienrecibeid_error3').style.color = '#ff3300';
                    error++;
                }
                else if (document.getElementById('emailquienrecibeid').value.length > 0) {
                    document.getElementById('fechaentregaid_error3').style.display = "block";
                    document.getElementById('fechaentregaid_error3').style.color = '#ff3300';
                    error++;
                }
                else {
                    document.getElementById('emailquienrecibeid_error3').style.display = "block";
                    document.getElementById('emailquienrecibeid_error3').style.color = '#ff3300';

                    document.getElementById('fechaentregaid_error3').style.display = "block";
                    document.getElementById('fechaentregaid_error3').style.color = '#ff3300';

                    error++;
                }
            }
        }
    }

   

    // Para Equipo Retirado

    if (document.getElementById("equiporetiradoid") != null) {
        if (document.getElementById("equiporetiradoid").checked == true) {

            if ((document.getElementById("fecharetiroid") != null) && (document.getElementById("rutquienretiraid") != null) && (document.getElementById("nombrequienretiraid") != null)) {

                if ((document.getElementById('fecharetiroid').value.length > 0) && (document.getElementById('rutquienretiraid').value.length > 0) && (document.getElementById('nombrequienretiraid').value.length > 0)) {
                    // Dejar pasar
                    document.getElementById('fecharetiroid_error3').style.display = "none";
                    document.getElementById('rutquienretiraid_error3').style.display = "none";
                    document.getElementById('nombrequienretiraid_error3').style.display = "none";
                }
                else {
                    if (document.getElementById('fecharetiroid').value.length > 0) {
                        document.getElementById('fecharetiroid_error3').style.display = "none";
                    }
                    else {
                        document.getElementById('fecharetiroid_error3').style.display = "block";
                        document.getElementById('fecharetiroid_error3').style.color = '#ff3300';
                        error++;
                    }

                    if (document.getElementById('rutquienretiraid').value.length > 0) {
                        document.getElementById('rutquienretiraid_error3').style.display = "none";
                    }
                    else {
                        document.getElementById('rutquienretiraid_error3').style.display = "block";
                        document.getElementById('rutquienretiraid_error3').style.color = '#ff3300';
                        error++;
                    }

                    if (document.getElementById('nombrequienretiraid').value.length > 0) {
                        document.getElementById('nombrequienretiraid_error3').style.display = "none";
                    }
                    else {
                        document.getElementById('nombrequienretiraid_error3').style.display = "block";
                        document.getElementById('nombrequienretiraid_error3').style.color = '#ff3300';
                        error++;
                    }
                }
            }
        }
    }
    

    return error;
}


function validateFormEquiposParaEntrega() {

    // Set error catcher
    var error = 0;


    error = error + validarParte2_campos();


    // Para incorporar check 'Equipo Entregado' o 'Equipo Retirado'

    if (document.getElementById("equipoentregadoid") != null) {

        if (document.getElementById('equipoentregadoid').checked == true) {

            // Valida campos de Entrega y Retira
            //error = error + validarParte2_estados();
        }
    }

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateFormEquiposParaRetiro() {

    // Set error catcher
    var error = 0;


    error = error + validarParte2_campos();


    // Para incorporar check 'Equipo Entregado' o 'Equipo Retirado'

    if (document.getElementById("equiporetiradoid") != null) {

        if (document.getElementById('equiporetiradoid').checked == true) {

            // Valida campos de Entrega y Retira
            //error = error + validarParte2_estados();
        }
    }

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateFormEquiposEnProduccion() {

    // Set error catcher
    var error = 0;

    if (document.getElementById("subestadoid") == null) {
    }
    else {
        if (document.getElementById('subestadoid').value.length == 0) {
            document.getElementById('subestadoid_error1').style.display = "block";
            document.getElementById('subestadoid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('subestadoid_error1').style.display = "none";
        }
    }

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function validateFormEquiposPostProduccion() {

    // Set error catcher
    var error = 0;

    if (document.getElementById("subestadoid") == null) {
    }
    else {
        if (document.getElementById('subestadoid').value.length == 0) {
            document.getElementById('subestadoid_error1').style.display = "block";
            document.getElementById('subestadoid_error1').style.color = '#ff3300';
            error++;
        }
        else {
            document.getElementById('subestadoid_error1').style.display = "none";
        }
    }

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}


function limpiarEquipoEntregadoId() {

    // Set error catcher
    var error = 0;

    // Nada por ahora

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}



function limpiarEquipoRetiradoId() {

    // Set error catcher
    var error = 0;

    // Nada por ahora

    // Don't submit form if there are errors
    if (error > 0) {
        return false;
    }

}



