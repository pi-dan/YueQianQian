using Castle.DynamicProxy;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Attributes;
using YueQianQian.Common.Extensions;
using YueQianQian.Model;

namespace YueQianQian.Common.Aop
{
    public class TransactionInterceptor : IInterceptor
    {
        IUnitOfWork _unitOfWork;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public TransactionInterceptor(UnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.HasAttribute<TransactionAttribute>())
            {
                InterceptTransaction(invocation, method);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private async void InterceptTransaction(IInvocation invocation, MethodInfo method)
        {
            try
            {
                var transaction = method.GetAttribute<TransactionAttribute>();
                _unitOfWork = _unitOfWorkManager.Begin(transaction.Propagation, transaction.IsolationLevel);
                invocation.Proceed();

                dynamic returnValue = invocation.ReturnValue;
                if (returnValue is Task)
                {
                    returnValue = await returnValue;
                }

                if (returnValue is ApiResult res && res.code != 0)
                {
                    _unitOfWork.Rollback();
                }
                else
                {
                    _unitOfWork.Commit();
                }
            }
            catch
            {
                _unitOfWork.Rollback();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
