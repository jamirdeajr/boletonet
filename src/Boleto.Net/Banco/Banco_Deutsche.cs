using BoletoNet.Util;
using System;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.487.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <author>  
    /// Jamir Déa Júnior
    /// </author>    
    internal class Banco_Deutsche : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Deutsche.
        /// </summary>
        internal Banco_Deutsche()
        {
            this.Codigo = 487;
            this.Digito = "2";
            this.Nome = "Deutsche Bank";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod11Deutsche(Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true), 9);
        }

        #region IBanco Members

        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///      1º campo
        ///          composto pelo código de Banco, código da moeda, as cinco primeiras posições do campo 
        ///          livre e o dígito verificador deste campo;
        ///      2º campo
        ///          composto pelas posições 6ª a 15ª do campo livre e o dígito verificador deste campo;
        ///      3º campo
        ///          composto pelas posições 16ª a 25ª do campo livre e o dígito verificador deste campo;
        ///      4º campo
        ///          composto pelo dígito verificador do código de barras, ou seja, a 5ª posição do código de 
        ///          barras;
        ///      5º campo
        ///          Composto pelo fator de vencimento com 4(quatro) caracteres e o valor do documento com 10(dez) caracteres, sem separadores e sem edição.
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {

            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV


            #region Campo 1

            string Grupo1 = string.Empty;

            string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
            string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
            string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
            string D1 = Mod10(BBB + M + CCCCC).ToString();

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);


            #endregion Campo 1

            #region Campo 2

            string Grupo2 = string.Empty;

            string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            string D2 = Mod10(CCCCCCCCCC2).ToString();

            Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = string.Empty;

            string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            string D3 = Mod10(CCCCCCCCCC3).ToString();

            Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            string D4 = _dacBoleto.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            string FFFF = FatorVencimento(boleto).ToString();

            //if (boleto.Carteira == "06" && !Utils.DataValida(boleto.DataVencimento))
            //    FFFF = "0000";

            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            string VVVVVVVVVV = valor.ToString("N2").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

            //if (Utils.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;

        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda
        ///    05 a 05 – 1 - Dígito verificador do Código de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 – 25 - Campo Livre
        /// 
        ///   *******
        /// 
        /// </summary>
        /// 
        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valor = boleto.ValorCobrado > boleto.ValorBoleto ? boleto.ValorCobrado : boleto.ValorBoleto;
            var valorBoleto = valor.ToString("N2").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            if (boleto.Carteira == "02" || boleto.Carteira == "03" || boleto.Carteira == "09" || boleto.Carteira == "19" || boleto.Carteira == "26") // Com registro
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.Carteira == "06" || boleto.Carteira == "16" || boleto.Carteira == "25") // Sem Registro
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}0000{2}{3}", Codigo.ToString(), boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                        FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
                }

            }
            else
            {
                throw new NotImplementedException("Carteira ainda não implementada.");
            }


            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }


        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecessário)
        ///    24 a 25 -  2 - Zeros
        ///    26 a 33 -  8 - Código do Cedente (Código do beneficiário)
        ///    34 a 44 - 11 - Nosso número com dígito. Caso o dígito do nosso número seja P, preencher com Zero
        
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}{4}", boleto.Cedente.ContaBancaria.Agencia, "00",
                boleto.Cedente.Codigo, boleto.NossoNumero, Mod10(boleto.NossoNumero));
                                            

            return FormataCampoLivre;
        }


        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }


        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}/{1}-{2}", Utils.FormatCode(boleto.Carteira, 3), boleto.NossoNumero, boleto.DigitoNossoNumero);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            // Deuttch Bank sempre 00? 

            //if (boleto.Carteira != "02" && boleto.Carteira != "03" && boleto.Carteira != "06" && boleto.Carteira != "09" && boleto.Carteira != "16" && boleto.Carteira != "19" && boleto.Carteira != "25" && boleto.Carteira != "26")
            //    throw new NotImplementedException("Carteira não implementada. Carteiras implementadas 02, 03, 06, 09, 16, 19, 25, 26.");

            //O valor é obrigatório para a carteira 03
            //if (boleto.Carteira == "03")
            //{
            //    if (boleto.ValorBoleto == 0)
            //        throw new NotImplementedException("Para a carteira 03, o valor do boleto não pode ser igual a zero");
            //}

            //O valor é obrigatório para a carteira 09
            //if (boleto.Carteira == "09")
            //{
            //    if (boleto.ValorBoleto == 0)
            //        throw new NotImplementedException("Para a carteira 09, o valor do boleto não pode ser igual a zero");
            //}
            //else if (boleto.Carteira == "06")
            //{
            //    boleto.ValorBoleto = 0;
            //}

            if (boleto.ValorBoleto == 0)
                throw new NotImplementedException("O valor do boleto não pode ser igual a zero");



            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 10)
            {
                boleto.NossoNumero = boleto.NossoNumero.Substring(0, 10);
            }
            else if (boleto.NossoNumero.Length < 10)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("A quantidade de dígitos da Agência " + boleto.Cedente.ContaBancaria.Agencia + ", são de 4 números.");
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 7)
                throw new NotImplementedException("A quantidade de dígitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", são de 07 números.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            //Verifica se data do processamento é valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            // Atribui o nome do banco ao local de pagamento
            if (string.IsNullOrEmpty(boleto.LocalPagamento))
                boleto.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA";


            // Calcula o DAC do Nosso Número
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _dacNossoNumero;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        #endregion IBanco Members

        private string Mod11Deutsche(string seq, int b)
        {
            #region Cálculo dígito do DEUTSCHE BANK
            /* 
            Para se calcular o dígito-verificador do nosso número, deve-se utilizar o
            Módulo 11, cujo detalhe é apresentado abaixo:
            1) Pegar o número do sequencial, completar com zeros à esquerda até que atinja o
            tamanho de 10 dígitos. Multiplicar, começando pela direita, pelo valor de um
            fator que sempre começará com o número ‘2’ (dois) e irá até o número ‘9’
            (nove). Quando o número do fator for maior que ‘9’ (nove) o fator volta para o
            valor de ‘2’ (dois) e assim por diante.
            2) Somar o resultado da multiplicação.
            3) Dividir a soma por 11.
            4) A diferença de 11 pelo resto da divisão acima será o digito verificador.
            */
            #endregion

            /* Variáveis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(seq.Mid(i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0 || r == 1)
                return "0";
            else
                return (11 - r).ToString();
        }

        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

        public string GerarRegistroDetalhe2(Boleto boleto, int numeroRegistro)
        {
            string _detalhe = "";
            _detalhe += "2";                                        // 001 a 001 Tipo Registro
            _detalhe += new string(' ', 320);                       // 002 a 321 Mensagens 1,2,3,4
            if (boleto.DataOutrosDescontos == DateTime.MinValue)    // 322 a 327 Data limite para concessão de Desconto 2
            {
                _detalhe += "000000"; //Caso nao tenha data de vencimento
            }
            else
            {
                _detalhe += boleto.DataOutrosDescontos.ToString("ddMMyy");
            }

            // 328 a 340 Valor do Desconto 2
            _detalhe += Utils.FitStringLength(boleto.OutrosDescontos.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
            _detalhe += "000000"; // 341 a 346 ata limite para concessão de Desconto 3
            // 347 a 359 Valor do Desconto 3
            _detalhe += Utils.FitStringLength("", 13, 13, '0', 0, true, true, true);
            _detalhe += new string(' ', 7);          // 360 a 366 Filler 
            _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);  // 367 a 369  Nº da Carteira 
            _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); // 370 a 374 N da agencia(5)
            _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true); // 375 a 381 Conta Corrente(7)
            _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);// 382 a 382 D da conta(1)
            _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); // 383 a 393 Nosso Número (11)
            // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012
            _detalhe += Mod11Deutsche(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // 394 a 394 Digito de Auto Conferencia do Nosso Número (01)
            //Desconto Bonificação por dia (10, N)
            _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // 395 a 400
            //Retorno
            return Utils.SubstituiCaracteresEspeciais(_detalhe);
        }

    }
}