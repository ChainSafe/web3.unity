enum ErrorCode
{
    NOUSERFOUND,
    ENCODING_ERROR,
    DECODING_ERROR,
    RUNTIME_ERROR,
    APP_CANCELLED,
    SOMETHING_WENT_WRONG,
}
sealed class Web3AuthError
{
    public static string getError(ErrorCode errorCode)
    {
        switch (errorCode)
        {
            case ErrorCode.NOUSERFOUND:
                return "No user found, please login again!";
            case ErrorCode.ENCODING_ERROR:
                return "Encoding Error";
            case ErrorCode.DECODING_ERROR:
                return "Decoding Error";
            case ErrorCode.SOMETHING_WENT_WRONG:
                return "Something went wrong!";
            case ErrorCode.RUNTIME_ERROR:
                return "Runtime Error";
            case ErrorCode.APP_CANCELLED:
                return "App Cancelled!";

        }
        return "";
    }
}
