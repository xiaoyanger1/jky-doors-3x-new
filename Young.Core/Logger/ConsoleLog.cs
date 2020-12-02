using System;

namespace Young.Core.Logger
{
    public class ConsoleLog : ILog
    {
        #region Implementation of ILog

        /// <summary>
        /// ��Ϣ
        /// </summary>
        /// <param name="obj"></param>
        public void Info(object obj)
        {
            Console.WriteLine(obj);
        }

        /// <summary>
        /// �쳣
        /// </summary>
        /// <param name="obj"></param>
        public void Exception(object obj)
        {
            Console.WriteLine(obj);
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="obj"></param>
        public void Debug(object obj)
        {
            Console.WriteLine(obj);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="obj"></param>
        public void Warn(object obj)
        {
            Console.WriteLine(obj);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="obj"></param>
        public void Error(object obj)
        {
            Console.WriteLine(obj);
        }

        #endregion
    }
}