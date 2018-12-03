using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Deutsche
    {
        DuplicataMercantil,
        NotaPromissoria,
        NotaSeguro,
        CobrancaSeriada,
        Recibo,
        LetraCambio,
        NotaDebito,
        DuplicataServico,
        Outros,
    }

    #endregion

    public class EspecieDocumento_Deutsche : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Deutsche()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Deutsche(string codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Deutsche.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Deutsche.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Deutsche.NotaSeguro: return "3";
                case EnumEspecieDocumento_Deutsche.CobrancaSeriada: return "4";
                case EnumEspecieDocumento_Deutsche.Recibo: return "5";
                case EnumEspecieDocumento_Deutsche.LetraCambio: return "10";
                case EnumEspecieDocumento_Deutsche.NotaDebito: return "11";
                case EnumEspecieDocumento_Deutsche.DuplicataServico: return "12";
                case EnumEspecieDocumento_Deutsche.Outros: return "99";
                default: return "99";

            }
        }

        public EnumEspecieDocumento_Deutsche getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1" : return EnumEspecieDocumento_Deutsche.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Deutsche.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Deutsche.NotaSeguro;
                case "4": return EnumEspecieDocumento_Deutsche.CobrancaSeriada;
                case "5": return EnumEspecieDocumento_Deutsche.Recibo;
                case "10": return EnumEspecieDocumento_Deutsche.LetraCambio;
                case "11": return EnumEspecieDocumento_Deutsche.NotaDebito;
                case "12": return EnumEspecieDocumento_Deutsche.DuplicataServico;
                case "99": return EnumEspecieDocumento_Deutsche.Outros;
                default: return EnumEspecieDocumento_Deutsche.Outros;
            }
        }

        public override string getCodigoEspecieBySigla(string sigla)
        {
            switch (sigla)
            {
                case "DM": return "1";
                case "NP": return "2";
                case "NS": return "3";
                case "CS": return "4";
                case "RC": return "5";
                case "LC": return "10";
                case "ND": return "11";
                case "DS": return "12";
                default: return "99";
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Deutsche();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Deutsche.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Deutsche.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Deutsche.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Deutsche.CobrancaSeriada:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.CobrancaSeriada);
                        this.Especie = "Cobrança seriada";
                        this.Sigla = "CS";
                        break;
                    case EnumEspecieDocumento_Deutsche.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Deutsche.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de câmbio";
                        break;
                    case EnumEspecieDocumento_Deutsche.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Deutsche.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Deutsche.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.Outros);
                        this.Especie = "Outros";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "( Selecione )";
                        this.Sigla = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            try
            {
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();

                EspecieDocumento_Deutsche obj = new EspecieDocumento_Deutsche();

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.DuplicataMercantil));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.NotaPromissoria));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.NotaSeguro));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.CobrancaSeriada));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.Recibo));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.LetraCambio));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.NotaDebito));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.DuplicataServico));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Deutsche(obj.getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.Outros));
                alEspeciesDocumento.Add(obj);

                return alEspeciesDocumento;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Deutsche(getCodigoEspecieByEnum(EnumEspecieDocumento_Deutsche.DuplicataMercantil));
        }

        #endregion
    }
}