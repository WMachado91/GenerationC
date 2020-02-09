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
    $("#CEP").mask("99999-999");
    $("#Estado").attr("disabled", "disabled");
    $("#Cidade").attr("disabled", "disabled");
    $("#Endereco").attr("disabled", "disabled");

    $("#XY").click(function () {
        definitarLatLng(address);
    })
        
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

        pesquisarPorCEP(cep);

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
    $("#Endereco").val("");
    $("#bairro").val("");
    $("#Cidade").val("");
    $("#Estado").val("");
    $("#ibge").val("");
}
function pesquisarPorCEP(cep) {

    var cep = cep.replace(/\D/g, '');

    if (cep != "") {
        var validacep = /^[0-9]{8}$/;

        if (validacep.test(cep)) {

            $("#Endereco").val("...");
            $("#Cidade").val("...");
            $("#Estado").val("...");

            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                if (!("erro" in dados)) {
                    $("#Endereco").val(dados.logradouro);
                    $("#Cidade").val(dados.localidade);
                    $("#Estado").val(dados.uf);
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