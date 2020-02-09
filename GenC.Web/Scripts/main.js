var latitude;
var longitude;
var ponteiro;
var address = "Rua Torquarto Dias, 350"//document.getElementById('address').value
var geocoder;
var data = new Date();

$(document).ready(function () {
    $("#loader").hide();

    $('.datepicker').datepicker({
        autoClose: true,
        today: "Hoje",
        selectMonths: true,
        selectYears: true,
        clear: false
    });

    $("#campo").click(function () {
        pesquisacep($(this).val());
    });

    $(".cpf").mask("999.999.999-99");  
    $(".CEP").mask("99999-999");
    $("#Estado").attr("disabled", "disabled");
    $("#Cidade").attr("disabled", "disabled");
    $("#Endereco").attr("disabled", "disabled");

    $("#XY").click(function () {
        definitarLatLng(address);
    })

    $('#tblHome').DataTable({
        ajax: {
            url: urlApiBanco + "/api/dashboard/ObtemGraficoProducao/",
            type: "POST",
            data: function (d) {
                d.filtros = filtroDashboard;
                d.filtros.page_size = d.length;
                d.filtros.page = paginaTabela;
                d.filtros.last = ultimaProposta;
                d.filtros.first = primeiraProposta;
                return JSON.stringify(d);
            },
            contentType: "application/json; charset=utf-8",
            crossDomain: true,
            dataType: "json",
            dataSrc: function (result) {
                if (jQuery.type(result) === "string") {
                    bootbox.alert(result);
                }
                else {
                    ultimaProposta = result.data.data[result.data.data.length - 1].id_proposta; //parametroUrl(result.data.pagination.links.next, "last");
                    primeiraProposta = result.data.data[0].id_proposta;  //parametroUrl(result.data.pagination.links.previous, "first"); 
                    $("#lblTotalPropostasEnviadas").html(result.data.totalPropostasEnviadas);
                    $("#EnvLblTotalPropostasEnviadas").html(result.data.valorTotalPropostasEnviadas.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));

                    $("#lblTotalContratosEmitidos").html(0);

                    $("#lblTotalPropostasAprovadas").html(result.data.totalPropostasAprovadas);
                    $("#aprovLblTotalPropostasAprovadas").html(result.data.valorTotalPropostasAprovadas.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));

                    $("#lblTotalPropostasCondicionadas").html(result.data.totalPropostasCondicionadas);
                    $("#EnvLblTotalPropostasCondicionadas").html(result.data.valorTotalPropostasCondicionadas.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));

                    $("#lblTotalPropostasCanceladas").html(result.data.totalPropostasCanceladas);
                    $("#EnvLblTotalPropostasCanceladas").html(result.data.valorTotalPropostasCanceladas.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));

                    $("#lblTotalPropostasEmAnalise").html(result.data.totalPropostasEmAnalise);
                    $("#EnvLblTotalPropostasEmAnalise").html(result.data.valorTotalPropostasEmAnalise.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' }));

                    return result.data.data;
                }
            }, error: function (xhr, error, thrown) {
                console.log(xhr);
            }
        },
        processing: true,
        serverSide: true,
        paging: true,
        pagingType: "full",
        destroy: true,
        scrollX: true,
        ordering: false,
        responsive: {
            details: true
        },
        select: false,
        searching: false,
        lengthMenu: [10, 20, 30],
        pageLength: 10,
        columns: [
            {
                data: 'id_proposta'
            },
            {
                render: function (data, type, full, meta) {
                    return '<a target="_blank" href="/Portal/pages/upload_documentos.html?prm_numProposta=' + full.id_proposta + '"><img src="../dist/img/search-engine.svg" style="width: 2em"></a>'
                }
            },
            {
                render: function (data, type, full, meta) {
                    return formataCPF(full.cpf_cliente);
                }
            },
            {
                data: 'nome_cliente'
            },
            {
                data: 'comercial.canal'
            },
            {
                render: function (data, type, full, meta) {
                    var date = full.datas.envio.replace(/\D/g, "-");
                    var dtFinal = new Date(date);
                    return dtFinal.toLocaleString('pt-BR', { year: "numeric", month: "numeric", day: "numeric" });
                }
            },
            {
                render: function (data, type, full, meta) {
                    return full.valores.credito_solicitado.toLocaleString('pt-br', { style: 'currency', currency: 'BRL' });;
                }
            },
            {
                data: 'status.descricao_credito'
            },
            {
                data: 'canal_entrada'
            }
        ],
        columnDefs: [
            {
                "searchable": false,
                "orderable": false,
                "targets": "_all"
            },
            {
                "targets": [0],
                "responsivePriority": 1
            },
            {
                "targets": [1],
                "responsivePriority": 2,
                "className": "center-td",
                "width": "3em"
            }, {
                "targets": [3],
                "visible": Mostra2
            },
            {
                "targets": [4],
                "visible": Mostra3
            }, {
                "targets": [7],
                "responsivePriority": 3
            },
            {
                "targets": [8],
                "responsivePriority": 4
            }
        ],

        language: {
            "decimal": "",
            "emptyTable": "Sem registros",
            "info": "Mostrando _START_ ate _END_ de _TOTAL_ regístros",
            "infoEmpty": "Mostrando 0 to 0 de 0 registros",
            "infoFiltered": "(filtrado de _MAX_ registros)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrando _MENU_ registros",
            "loadingRecords": "Carregando...",
            "processing": "Processando...",
            "search": "Busca:",
            "zeroRecords": "Nenhum registro correspondente encontrado",
            "paginate": {
                "first": "Primeiro",
                "last": "Ultimo",
                "next": "   Proximo",
                "previous": "Anterior    "
            },
            aria: {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }
        }
    });
    $("#DtNascimento").change(function () {
        var dtNascimento = $("#DtNascimento").val();
        var dtNascAno = new Date(dtNascimento).toLocaleString('pt-BR', { year: "numeric" });
        var anoAtual = new Date().toLocaleString('pt-BR', { year: "numeric" });

        var anoInt = parseInt(anoAtual);
        var dtNascAnoInt = parseInt(dtNascAno);

        if (anoInt === NaN || dtNascAnoInt === NaN || anoInt <= 0 || dtNascAnoInt <= 0) {
            alert("Ano incorreto");
            $(this).val("");
            $(this).focus();
        }
        else if (anoInt - dtNascAnoInt < 18) {
            console.log(anoInt - dtNascAnoInt)
            alert("Você deve ter pelo menos 18 anos.")
            $(this).val("");
            $(this).focus();
        }
    });

    $("#CEP").change(function () {
        var cep = $("#CEP").val().replace(/\D/g, "");
        $.ajax({
            type: "GET",
            url: `viacep.com.br/ws/${cep}/json/`,
            contentType: "application/json; charset=utf-8",
            crossDomain: true,
            dataType: "json",
            success: function (res) {
                $("#Estado").val(res.UF);
                $("#Cidade").val(res.localidade);
                $("#Endereco").val(res.logradouro);
            }
        })
    });

    $("#btnEnviar").click(function (e) {
        var evento = {};

        $.ajax({
            type: "POST",
            url: "\Consulta\castrarEvento",
            contentType: "application/json; charset=utf-8",
            async: false,
            data: JSON.stringify(evento),
            crossDomain: true,
            dataType: "json",
            success: function (res) {
                toastr.success(res);
            },
            error: function (e) {

                toastr.error(e);
            }
        })
    });

    $("#CadastroFrm").on('submit', function (e) {
        var confirmacao = confirm("Todos os dados estão corretos e deseja continuar?");
        if (!confirmacao) {
            $("#Nome").focus();
            return false;
        }
    });

    $(document).submit(function (e) {
  
        $("#loader").show();
    })
});

function limpa_formulario_cep() {
    $("#rua").val("");
    $("#bairro").val("");
    $("#cidade").val("");
    $("#uf").val("");
    $("#ibge").val("");
}
function pesquisarPorCEP(cep) {

    var cep = cep.replace(/\D/g, '');

    if (cep != "") {
        var validacep = /^[0-9]{8}$/;

        if (validacep.test(cep)) {

            $("#rua").val("...");
            $("#bairro").val("...");
            $("#cidade").val("...");
            $("#uf").val("...");
            $("#ibge").val("...");

            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                if (!("erro" in dados)) {
                    $("#rua").val(dados.logradouro);
                    $("#bairro").val(dados.bairro);
                    $("#cidade").val(dados.localidade);
                    $("#uf").val(dados.uf);
                    $("#ibge").val(dados.ibge);
                }

                else {
                    limpa_formulário_cep();
                    alert("CEP não encontrado.");
                }
            });
        }
        else {
            limpa_formulário_cep();
            alert("Formato de CEP inválido.");
        }
    }
}

function initMap() {

    var mapOptions = {
        center: { lat: latitude, lng: longitude },
        zoom: 15,
        disableDefaultUI: true,
        zoomControl: true
    };

    var map = new google.maps.Map(document.getElementById('map'), mapOptions);

    var marker = new google.maps.Marker({
        position: { lat: latitude, lng: longitude },
        map: map
    });

    $("#map").show();
}

function definitarLatLng(endereco) {
    var ret;
    geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': address, 'region': 'BR' }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            ret = 1;
            if (results[0]) {
                latitude = results[0].geometry.location.lat();
                longitude = results[0].geometry.location.lng();
            }
        }
        initMap();
    });
}