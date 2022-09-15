namespace Shared
{
    public static class Constants
    {
        public static class AppSettings
        {
            public const string AzureServiceBusConfig = "AzureServiceBus";
            public const string LogstashConfig = "LogstashConfig";
        }
        
        public static class EventLog
        {
            public enum EventCode
            {
                TRACKING_ID_INFO,
                PSP_LOG,
                IPN_LOG,
                STATUS_LOG,
                EMAIL_LOG,
                UNEXPECTED_EXCEPTION,
                PAYMENT_SAVE_ERROR,
                PAYMENT_IPN_LOG_SAVE_ERROR,
                PAYMENT_PENDING_IPN_SAVE_ERROR,
                PAYMENT_PSP_GATEWAY_SAVE_ERROR,
                PAYMENT_STATUS_LOG_SAVE_ERROR,
                PAYMENT_PSP_LOG_SAVE_ERROR,
                PAYMENT_PSP_REFERENCE_SAVE_ERROR,
                SUBSCRIBER_GET_ENTITY,
                PAYMENT_PENDING_IPN_SEND_ERROR,
                PROCESS_PAYMENT_PENDING_IPN_ERROR,
                PAYMENT_PSP_WEBHOOK_PROCESS_ERROR,
                SEND_EMAIL_EXCEPTION,
                PAYMENT_PSP_BLL_ERROR,
                PAYMENT_PSP_BLL_EXCEPTION,
                PSP_PROCESS_WEBHOOK_ERROR,
                PSP_SAVE_WEBHOOK_ERROR,
                UPLOAD_BLOB_ERROR,
                DOWNLOAD_BLOB_ERROR,
                LOG_GET_LIST_ERROR,
                AUTHORIZATION,
            }

            public enum LogLevel
            {
                ERROR,
                WARN,
                INFO,
                DEBUG
            }

            public enum LogType
            {
                DATABASE,
                FILE
            }
        }

        public static class EnvironmentCode
        {
            public const string DEV = "DEV";
            public const string STG = "STG";
            public const string PRD = "PRD";
        }

        public static class LogLevelCode
        {
            public const string ERROR = "ERROR";
            public const string WARN = "WARN";
            public const string INFO = "INFO";
            public const string DEBUG = "DEBUG";
        }

        public static class LogEventCode
        {
            public const string APP = "APP";
            public const string TASK = "TASK";
            public const string EMAILING = "EMAILING";
            public const string INFO = "INFO";

            public const string LOGINFAIL = "LOGINFAIL";
        }

        public static class EmailTemplateCode
        {
            public const string PAYMENT_CREATED = "PAYMENT_CREATED";
            public const string CLAIM = "CLAIM";
            public const string PAYMENT_APPROVED = "PAYMENT_APPROVED";
        }

        public static class EmailStatusCode
        {
            public const string QUEUED = "QUEUED";
            public const string SENT = "SENT";
            public const string ERROR = "ERROR";
        }


        public static class ContentTypes
        {
            public const string ApplicationJson = "application/json";
            public const string ApplicationPdf = "application/pdf";
            public const string TextHtml = "text/html";
            public const string TextPlain = "text/plain";
        }

        public static class EncryptionKeys
        {
            public const string General = "SDCLKJYAFS654ASF321FP87K";
            public const string General2 = "TS7YYC214GTC831VW2NOE187";
        }

        public static class FormatDate
        {
            public const string DDMMYYY = "MM-dd-yyyy";
            public const string DDMMYYY2 = "MMMM, dd yyyy";
            public const string HOUR = "hh:mm tt";
            public const string FORMATDETAIL = "yyyy-MM-dd H:mm tt";
        }
    }
}