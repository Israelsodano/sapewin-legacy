using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SincronizaFuncoes
{
    public class Sincroniza
    {
        static public void Go()
        {
            // Se for Sapewin
            if (ConfigurationManager.AppSettings["Aplicacao"].ToUpper() == "SAPEWIN") 
            {
                
                Sapewin.SapewinContext Bank = new Sapewin.SapewinContext();
                Painel.PainelContext BankPainel = new Painel.PainelContext();
                Painel.Models.LoginModel Painel = new Painel.Models.LoginModel();
                Painel.Models.LoginSistema UsuarioLogado = Painel.LoginSistema.Include(x => x.Cliente).ThenInclude(x => x.Servidor).FirstOrDefault(x => x.IDLoginsistema == Convert.ToInt32(HttpContext.Current.Session["Usuario"].ToString()));

                // Lista Ids de Telas do Banco Login 
                List<int> IDsTelas = BankPainel.Telas.Where(x=>x.IDProduto == 1).Select(x => x.IDTela).ToList();

                // Refiltra esses Ids para que me retorne os Ids que NAO estão dento do banco do Sapewin
                IDsTelas = IDsTelas.Where(x => !Bank.Telas.Select(z => z.IDTela).Contains(x)).ToList();

                // para cada um desses Ids ^|^
                foreach (var ID in IDsTelas)
                {
                    // Carrego a Tela que Representa esse ID ^|^
                    Painel.Telas TelaPainel = BankPainel.Telas.FirstOrDefault(x => x.IDTela == ID);
                    
                    // Crio Essa Tela para o Sapewin
                    Sapewin.Telas TelaSapewin = new Sapewin.Telas
                    {
                        IDTela = TelaPainel.IDTela,
                        Nome = TelaPainel.Nome

                    };
                    // Salvo
                    Bank.Telas.Add(TelaSapewin);
                }

                //Listo os Ids das Funçoes existentes no Banco Login
                List<int> IDsFuncoes = BankPainel.Funcoes.Where(x => x.IDProduto == 1).Select(x => x.IDFuncao).ToList();

                //Refiltro esses Ids para retornar os Ids que eu NAO tenho dentro do Sapewin
                IDsFuncoes = IDsFuncoes.Where(x => !Bank.Funcoes.Select(z => z.IDFuncao).Contains(x)).ToList();

                // para cada um desses Ids ^|^
                foreach (var ID in IDsFuncoes)
                {
                    // carrego a Funcao que Representa esse ID ^|^
                    Painel.Funcoes FuncaoPainel = BankPainel.Funcoes.FirstOrDefault(x => x.IDFuncao == ID);

                    // Crio A Funcao no Sapewin
                    Sapewin.Funcoes FuncaoSapewin = new Sapewin.Funcoes
                    {
                        IDFuncao = FuncaoPainel.IDFuncao,
                        Nome = FuncaoPainel.Nome
                    };
                    // Salvo
                    Bank.Funcoes.Add(FuncaoSapewin);
                }

                // Listo os Ids das Funçoes de Telas no Login 
                List<String> IDsFucoesdeTelas = BankPainel.FuncoesdeTelas.Where(x => x.IDProduto == 1).Select(x => x.IDFuncaoTela).ToList();

                // Refiltro esse Ids para Retornar apenas o que nao existem no Sapewin
                IDsFucoesdeTelas = IDsFucoesdeTelas.Where(x => !Bank.FuncoesdeTelas.Select(z => z.IDFuncaoTela).Contains(x)).ToList();

                // para cada um desses ids ^|^
                foreach (var ID in IDsFucoesdeTelas)
                {
                    //Carrego a Funcao de tela que representa esse id 
                    Painel.FuncoesDeTelas FuncoesdeTelasPainel = BankPainel.FuncoesdeTelas.FirstOrDefault(x => x.IDFuncaoTela == ID);
                    // Crio a funcao de tela para o sapewin
                    Sapewin.FuncoesdeTelas FuncoesdeTelasSapewin = new Sapewin.FuncoesdeTelas
                    {
                        IDFuncaoTela = FuncoesdeTelasPainel.IDFuncaoTela,
                        IDTela = FuncoesdeTelasPainel.IDTela,
                        IDFuncao = FuncoesdeTelasPainel.IDFuncao
                    };
                    foreach (var empresa in Bank.Empresas.Select(x=>x.IDEmpresa).ToList())
                    {                        
                        foreach (var usuario in Painel.LoginSistema.Where(x=>x.IDCliente == UsuarioLogado.IDCliente))
                        {
                            Sapewin.PermissoesdeTelas PermissoesTela = new Sapewin.PermissoesdeTelas
                            {
                                IDUsuario = usuario.IDLoginsistema,
                                IDFuncaoTela = FuncoesdeTelasSapewin.IDFuncaoTela,
                                IDEmpresa = empresa,
                            };
                            Bank.PermissoesdeTelas.Add(PermissoesTela);
                        }
                    }
                        Bank.FuncoesdeTelas.Add(FuncoesdeTelasSapewin);                   
                }
                //salvo               
                Bank.SaveChanges();

                //se for DpaRep  A mesma logica que a anterior
            }else if(ConfigurationManager.AppSettings["Aplicacao"].ToUpper() == "DPAREP")
            {
                DpaRep.DparepContext Bank = new DpaRep.DparepContext();
                Painel.PainelContext BankPainel = new Painel.PainelContext();
                
                // Telas
                List<int> IDsTelas = BankPainel.Telas.Where(x => x.IDProduto == 2).Select(x => x.IDTela).ToList();

                IDsTelas = IDsTelas.Where(x => Bank.Telas.Select(z => z.IDTela).Contains(x)).ToList();

                foreach (var ID in IDsTelas)
                {
                    Painel.Telas TelaPainel = BankPainel.Telas.FirstOrDefault(x => x.IDTela == ID);

                    DpaRep.Telas TelaDparep = new DpaRep.Telas 
                    {
                        IDTela = TelaPainel.IDTela,
                        Nome = TelaPainel.Nome

                    };
                    Bank.Telas.Add(TelaDparep);
                }

                //Funcoes
                List<int> IDsFuncoes = BankPainel.Funcoes.Where(x => x.IDProduto == 2).Select(x => x.IDFuncao).ToList();

                IDsFuncoes = IDsFuncoes.Where(x => Bank.Funcoes.Select(z => z.IDFuncao).Contains(x)).ToList();

                foreach (var ID in IDsFuncoes)
                {
                    Painel.Funcoes FuncaoPainel = BankPainel.Funcoes.FirstOrDefault(x => x.IDFuncao == ID);

                    DpaRep.Funcoes FuncaoDparep = new DpaRep.Funcoes
                    {
                        IDFuncao = FuncaoPainel.IDFuncao,
                        Nome = FuncaoPainel.Nome
                    };
                    Bank.Funcoes.Add(FuncaoDparep);
                }

                //Funcoes de Telas
                List<String> IDsFucoesdeTelas = BankPainel.FuncoesdeTelas.Where(x => x.IDProduto == 2).Select(x => x.IDFuncaoTela).ToList();

                IDsFucoesdeTelas = IDsFucoesdeTelas.Where(x => Bank.FuncoesdeTelas.Select(z => z.IDFuncaoTela).Contains(x)).ToList();

                foreach (var ID in IDsFucoesdeTelas)
                {
                    Painel.FuncoesDeTelas FuncoesdeTelasPainel = BankPainel.FuncoesdeTelas.FirstOrDefault(x => x.IDFuncaoTela == ID);

                    DpaRep.FuncoesdeTelas FuncoesdeTelasDparep = new DpaRep.FuncoesdeTelas
                    {
                        IDFuncaoTela = FuncoesdeTelasPainel.IDFuncaoTela,
                        IDTela = FuncoesdeTelasPainel.IDTela,
                        IDFuncao = FuncoesdeTelasPainel.IDFuncao
                    };
                    foreach (var empresa in Bank.Empresas.Select(x=>x.IDEmpresa).ToList())
                    {
                        foreach (var usuario in Bank.PermissoesdeTelas.Select(x=>x.IDUsuario).Distinct().ToList())
                        {
                            DpaRep.PermissoesdeTelas permissoesdetela = new DpaRep.PermissoesdeTelas
                            {
                                IDEmpresa = empresa,
                                IDUsuario = usuario,
                                IDFuncaoTela = FuncoesdeTelasDparep.IDFuncaoTela,
                                Empresa = Bank.Empresas.Find(empresa),
                                FuncaodeTela = Bank.FuncoesdeTelas.FirstOrDefault(x => x.IDFuncaoTela == FuncoesdeTelasDparep.IDFuncaoTela)                               
                            };
                            Bank.PermissoesdeTelas.Add(permissoesdetela);
                        }
                    }
                    Bank.FuncoesdeTelas.Add(FuncoesdeTelasDparep);
                }
              Bank.SaveChanges();
            }
        }
    }
}
