using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TesteException.Exceptions
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory, string NomeAplicacao)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        RetornoExcecao retornoExcecao = null;

                        if (exceptionHandlerFeature.Error is ExcecaoBase)
                        {
                            var excecaoBase = exceptionHandlerFeature.Error as ExcecaoBase;

                            retornoExcecao = new RetornoExcecao
                            {
                                CodigoErro = excecaoBase.CodigoErro,
                                Mensagem = excecaoBase.Mensagem
                            };
                        }
                        else
                        {
                            retornoExcecao = new RetornoExcecao
                            {
                                CodigoErro = "erro_nao_tratado",
                                Mensagem = "Ocorreu um erro não tratado"
                            };
                        }

                        var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");

                        var objetoLog = new
                        {
                            CodigoErro = retornoExcecao.CodigoErro,
                            Mensagem = retornoExcecao.Mensagem,
                            MensagemException = exceptionHandlerFeature.Error.Message,
                            TrexoErro = exceptionHandlerFeature.Error.StackTrace,
                            ExcecaoInterna = exceptionHandlerFeature.Error.InnerException
                        };

                        logger.LogError(exceptionHandlerFeature.Error, JsonConvert.SerializeObject(objetoLog), new object[0]);

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(retornoExcecao));
                    }
                });
            });
        }
    }

    public class RetornoExcecao
    {
        public string CodigoErro { get; set; }
        public string Mensagem { get; set; }
    }
}
