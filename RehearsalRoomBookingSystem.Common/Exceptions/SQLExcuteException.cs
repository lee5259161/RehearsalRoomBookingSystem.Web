namespace RehearsalRoomBookingSystem.Common.Exceptions
{
    /// <summary>
    /// 自訂DB 執行錯誤Exception
    /// </summary>
    public class SQLExcuteException : Exception
    {
        private string _paramName;

        /// <summary>Initializes a new instance of the <see cref="SQLExcuteException" /> class.</summary>
        public SQLExcuteException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="SQLExcuteException" /> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public SQLExcuteException(string message) : base(message)
        {
        }

        /// <summary>
        /// 回傳如以下規格錯誤訊息<br/>
        /// 錯誤訊息 (Parameter '參數')
        /// 範例： {"memberId cannot be null (Parameter 'memberId')"}
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public SQLExcuteException(string message, string paramName) : base(message)
        {
            _paramName = paramName;
        }

        /// <summary>Gets the name of the parameter.</summary>
        /// <value>The name of the parameter.</value>
        public virtual string ParamName
        {
            get { return _paramName; }
        }

        /// <summary>Gets a message that describes the current exception.</summary>
        public override string Message
        {
            get
            {
                if (this._paramName == null)
                {
                    return base.Message;
                }
                else
                {
                    return $"{base.Message} (Parameter '{this._paramName}')";
                }
            }
        }
    }
}
