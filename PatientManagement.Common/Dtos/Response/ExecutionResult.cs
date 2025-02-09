using System.Collections.Generic;
using System.ComponentModel;

namespace PatientManagement.Common.Dtos.Response
{
   

    
        public class ExecutionResult
        {
            public ExecutionResult()
            {
                Response = ResponseCode.Ok;
            }

            public ResponseCode Response { get; set; }
            public string Message { get; set; }
            public string UserMessage { get; set; }
            public int RetryCount { get; set; }
            public List<string> ValidationMessages { get; set; }
        }

        public class ExecutionResult<T> : ExecutionResult
        {
            public T Result { get; set; }

            public static ExecutionResult<T> Success(T result, string message)
            {
                ExecutionResult<T> response = new ExecutionResult<T> { Response = ResponseCode.Ok, Result = result, Message = message, UserMessage = message };

                return response;
            }

            /// <summary>
            /// Creates a failed result. It takes no result object
            /// </summary>
            /// <param name="errorMessage">The error message returned with the response</param>
            /// <returns>The created response object</returns>
            public static ExecutionResult<T> Failed(string errorMessage, ResponseCode responseCode = ResponseCode.Failed)
            {
                ExecutionResult<T> response = new ExecutionResult<T> { Response = responseCode, Message = errorMessage };

                return response;
            }

            public static ExecutionResult<T> Failed(string errorMessage)
            {
                var response = new ExecutionResult<T> { Response = ResponseCode.ProcessingError, Message = errorMessage };

                return response;
            }

            public static ExecutionResult<T> Processing(string errorMessage)
            {
                ExecutionResult<T> response = new ExecutionResult<T> { Response = ResponseCode.Processing, Message = errorMessage };

                return response;
            }
        }

        public enum ResponseCode
        {
            [Description("Request was successful")]
            Ok = 0,

            [Description("Invalid details supplied")]
            ValidationError = 1,

            [Description("No record found")]
            NotFound = 2,

            [Description("Request failed. Please try again")]
            Failed = 3,

            [Description("Authentication failed. Please try again with the right credentials")]
            AuthorizationError = 4,

            [Description("You account balance is insufficient for this transaction")]
            InsufficientFund,

            [Description("Daily Limit exceeded")]
            DailyLimitExceeded,

            [Description("Duplicate transaction. Please try again in 5 minutes")]
            RepeatedTransaction,

            [Description("Invalid account details. Please try again")]
            InvalidAccountNumber,

            [Description("Invalid image validation. Please try again")]
            InvalidFaceValue,

            [Description("Please enter the OTP sent to you")]
            OTPRequired,

            [Description("Invalid OTP. Please enter the four digits code sent to you")]
            InvalidOTP,

            [Description("Incorrect PIN. Please try again")]
            InvalidPIN,

            [Description("Invalid card details. Please try again")]
            InvalidCardValue,

            [Description("Invalid hardware token. Please try again")]
            InvalidHardTokenValue,

            [Description("Invalid Biometric, please try again")]
            InvalidFingerPrintValue,

            [Description("Your PIN is locked, click on reset Transaction PIN to unlock.")]
            PinLocked,

            [Description("Name enquiry failed. Please try again")]
            InvalidNameEnquiry,

            [Description("Payment has been previously processed")]
            DuplicateRecord,

            [Description("Platform limit exceeded")]
            ChannelLimitExceeded,

            [Description("You have only # left out of your daily platform limit.Please try a transaction that does not exceed that amount.")]
            CustomerLimitExceeded,

            [Description("Platform limit exceeded")]
            NoChannelConfiguration,

            [Description("Platform limit exceeded")]
            MissingChallenge,

            [Description("Duplicate Account details")]
            DuplicateAccountDetails,

            [Description("Transaction is in progress")]
            Processing,

            [Description("Cross currency account transaction is not permitted")]
            CrossCurrency,

            [Description("Re-Initiate Transfer")]
            ReInitiateTransfer,

            [Description("We cannot confirm the status of the transaction at this time. Please check with the beneficiary before you try again")]
            Dispute,

            [Description("Transaction requires futher validation")]
            AdditionalValidationRequired,

            [Description("2FA limit exceeded")]
            ChallengeLimitExceeded,

            [Description("Transaction Initiated")]
            ApprovalInitiated,

            [Description("Processing Error")]
            ProcessingError,

            [Description("Expired payment session")]
            ExpiredPaymentSession,

            [Description("Settlement created successfully")]
            SettlementCreated,

            [Description("Profile blocked")]
            ProfileBlockedFirstTransaction
        }

        public class PagedList<T> where T : class
        {
            public T Data { get; set; }
            public int TotalCount { get; set; }

            public int PageSize { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal TotalConvertedAmount { get; set; }
        }

        public class NewPagedList<T> where T : class
        {
            public NewPagedList()
            {
                Data = new List<T>();
            }

            public List<T> Data { get; }

            public int TotalCount { get; set; }

            public int PageSize { get; set; }
        }
    
}
