
var visualiza = 10;
var Pagina = 0;
var order;

var cbxEmpresas = 0;
var SetoresSelecionados = new Array();
var DepartamentosSelecionados = new Array();
var FuncionariosSelecionados = new Array();
var FuncionariosNaoslc = new Array();
var EmpresasSelecionadas = new Array();


function Recarregajs(boolean) {
    window.location.reload(boolean);
    boolean = false;
}

function RetornaPadrao() {
    $('#AlertaGeral').attr('hidden', 'hidden');
    cbxEmpresas = 0;
    SetoresSelecionados = new Array();
    DepartamentosSelecionados = new Array();
    FuncionariosSelecionados = new Array();
    FuncionariosNaoslc = new Array();
    EmpresasSelecionadas = new Array();

    $('#BodyGeral').html(' <div class="container-fluid align-items-center text-center align-self-center flex-column d-flex justify-content-center" style="height:' + $('#Mywrapper').height() + 'px;" ><i class="fa fa-spinner fa-spin fa-5x fa-fw"></i></div>');
    $('#BodyGeral').load('/CadastroUsuarios/CadastroUsuarios_Abrir', function (r, s, h) {
        if (s == 'error') {
            $('#txtalertaGeral').text('Erro: ' + h.status + " : " + h.statusText);
            $('#AlertaGeral').removeAttr('hidden');$('#Mywrapper').animate({ scrollTop: 0 }, 1000);$('#Alerta').removeAttr('hidden');$('#Mywrapper').animate({ scrollTop: 0 }, 1000);
        }
    });
}

$('#FechaAlertaGeral').click(function () {
    $(this).parent().attr('hidden', 'hidden');

});

function Ordenador() {
    $('.ordenador').each(function (idx, el) {
        if ($(el).attr('order').includes(order.replace('-up', '').replace('-down', ''))) {
            $(el).children().attr('style', 'color:cornflowerblue;')
            if (order.includes('down')) {
                $(el).children().children().removeClass('fa-caret-up');
                $(el).children().children().addClass('fa-caret-down');
            }
        }
    });
}

$('#BodyGeral').on('change', '.Seletor', function () {
    if($(this).val() == 'oitenta'){ visualiza = 80; }else if($(this).val() == 'vinte'){ visualiza = 20; }else if($(this).val() == 'quarenta'){ visualiza = 40; }else{ visualiza = 10; }    
    $($(this).attr('Load')).load($(this).attr('Evento') + $('.txtPesquisar').val() + '&Range=' + visualiza + '&Pagina=' + Pagina + '&Order=' + order + "&Condicao=" + $('#Contem').is(':checked'));
});

function LoadGridCadastroCargos() {
    $('#TabCadastroCargos').load('/CadastroCargos/CadastroCargos_Abrir_Grid/?pesquisa=' + ($('.txtPesquisar').val() != null ? $('.txtPesquisar').val().replace(' ', '+') : "") + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroEmpresas() {
    $('#TabCadastroEmpresas').load('/CadastroEmpresas/CadastroEmpresas_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroSetores() {
    $('#TabCadastroSetores').load('/CadastroSetores/CadastroSetores_Abrir_Grid/?pesquisa=' + ($('.txtPesquisar').val() != null ? $('.txtPesquisar').val().replace(' ', '+') : "") + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridSelecionaEmpresa() {
    $('#TabEmpresas').load('/Home/SelecionaEmpresa_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroDepartamentos() {
    $('#TabCadastroDepartamentos').load('/CadastroDepartamentos/CadastroDepartamentos_Abrir_Grid/?pesquisa=' + ($('.txtPesquisar').val() != null ? $('.txtPesquisar').val().replace(' ', '+') : "") + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroUsuarios() {
    $('#TabCadastroUsuarios').load('/CadastroUsuarios/CadastroUsuarios_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroFeriadosGerais() {
    $('#TabCadastroFeriadosGerais').load('/CadastroFeriadosGerais/CadastroFeriadosGerais_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked') + "&Ano=" + $('#Ano').val());
}

function LoadGridCadastroFeriadosEspecificos() {
    $('#TabCadastroFeriadosEspecificos').load('/CadastroFeriadosEspecificos/CadastroFeriadosEspecificos_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroMotivosdeAbono() {
    $('#TabCadastroMotivosdeAbono').load('/CadastroMotivosdeAbono/CadastroMotivosdeAbono_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroHorarios() {
    $('#TabCadastroHorarios').load('/CadastroHorarios/CadastroHorarios_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroEscalas() {
    $('#TabCadastroEscalas').load('/CadastroEscalas/CadastroEscalas_Abrir_Grid/?pesquisa=' + ($('.txtPesquisar').val() != null ? $('.txtPesquisar').val().replace(' ', '+') : "") + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroUsuarios() {
    $('#TabCadastroUsuarios').load('/CadastroUsuarios/CadastroUsuarios_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function AtualizaGridFuncionarios() {
    $('#TabFuncionarios').html(' <div class="container-fluid align-items-center text-center align-self-center flex-column d-flex justify-content-center" style="height:500px;" ><i class="fa fa-spinner fa-spin fa-5x fa-fw"></i></div>');
    $('#TabFuncionarios').load('/CadastroUsuarios/GridFuncionarios/?id=' + cbxEmpresas, { Setores: SetoresSelecionados, Departamentos: DepartamentosSelecionados, pesquisa: $('#txtPesquisarFuncionario').val().replace(' ', '+') });
}

function LoadGridCadastroParametros() {
    $('#TabCadastroParametros').load('/CadastroParametros/CadastroParametros_Abrir_Grid/?pesquisa=' + ($('.txtPesquisar').val() != null ? $('.txtPesquisar').val().replace(' ', '+') : "") + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroFuncionarios() {
    $('#TabCadastroFuncionarios').load('/CadastroFuncionarios/CadastroFuncionarios_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroMensagens() {
    $('#TabCadastroMensagens').load('/CadastroMensagens/CadastroMensagens_Abrir_Grid/?pesquisa=' + $('.txtPesquisar').val().replace(' ', '+') + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function LoadGridCadastroTeste() {
    $('#TabCadastroTeste').load('/CadastroTeste/CadastroTeste_Abrir_Grid/?pesquisa=' + ($('.txtPesquisar').val() != null ? $('.txtPesquisar').val().replace(' ', '+') : "") + "&Range=" + visualiza + "&Pagina=" + Pagina + "&Order=" + order + "&Condicao=" + $('#Contem').is(':checked'));
}

function Tempo_Minuto(Times) {

    Minutes = new Array();

    for (var i = 0; i < Times.length; i++) {
        Minutes[i] = (parseInt(Times[i].split(':')[0]) * 60) + parseInt(Times[i].split(':')[1]);
    }

    return Minutes;
   
}

function Minuto_Tempo(Minutos) {

    Times = new Array();

    for (var i = 0; i < Minutos.length; i++) {
        Times[i] = (Math.floor(Minutos[i] / 60) < 10 ? '0' + Math.floor(Minutos[i] / 60) : '' + Math.floor(Minutos[i] / 60)) + ":" + (Minutos[i] % 60 < 10 ? '0' + Minutos[i] % 60 : '' + Minutos[i] % 60);
    }

    return Times;
}

function SomaHoras_Minutos(Times) {

    var Minutes = 0;

    for (var i = 0; i < Times.length; i++) {

        Minutes = Minutes + parseInt(Times[i].split(':')[0]) * 60 + parseInt(Times[i].split(':')[1]);
    }

    return Minutes;
}

function SomaHoras_Tempo(Minutes) {

    var Time = 0;

    for (var i = 0; i < Minutes.length; i++) {

        Time = Time + Minutes[i];
    }

    return (Math.floor(Time / 60) < 10 ? '0' + Math.floor(Time / 60) : '' + Math.floor(Time / 60)) + ':' + (Time % 60 < 10 ? '0' + Time % 60 : '' + Time % 60);
}

function monitorador() {

    var target = document.getElementsByClassName('txtalerta');

    var observer = new MutationObserver(function (mutations) {
        setTimeout(function () { $('.alert').attr('hidden', 'hidden') }, 3000);
    });
    var config = { attributes: true, childList: true, characterData: true };

    for (var i = 0; i < target.length; i++) {
        observer.observe(target[i], config);
    }
}