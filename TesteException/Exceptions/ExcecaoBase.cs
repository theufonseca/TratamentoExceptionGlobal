using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteException.Exceptions
{
    public class ExcecaoBase : Exception
    {
        public string CodigoErro { get; protected set; }
        public string Mensagem { get; protected set; }
        public object Data { get; protected set; }

        public ExcecaoBase()
        {
            
        }
    }

    public class ExcecaoErroAoGerarPedido : ExcecaoBase
    {
        public ExcecaoErroAoGerarPedido()
        {
            CodigoErro = "erro_ao_gerar_pedido";
            Mensagem = "Erro na geração do pedido";
        }
    }
}
