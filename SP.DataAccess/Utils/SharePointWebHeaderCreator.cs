using System.Net;

namespace SP.DataAccess.Utils
{
    public static class SharePointWebHeaderCreator
    {
        public static WebHeaderCollection GetAuthHeaders()
        {
            return new WebHeaderCollection
            {
                { "X-FORMS_BASED_AUTH_ACCEPTED", "f" },
                { HttpRequestHeader.ContentType, "application/json;odata=verbose" },
                { HttpRequestHeader.Accept, "application/json;odata=verbose" },
            };
        }

        public static WebHeaderCollection GetInsertHeaders(string formDigestValue)
        {
            return new WebHeaderCollection
            {
                { HttpRequestHeader.Accept, "application/json;odata=verbose" },
                { HttpRequestHeader.ContentType, "application/json;odata=verbose" },
                { "X-RequestDigest", formDigestValue },
            };
        }

        public static WebHeaderCollection GetUpdateHeaders(string formDigestValue)
        {
            return new WebHeaderCollection
            {
                { "X-HTTP-Method", "MERGE" }, { "IF-MATCH", "*" }, { HttpRequestHeader.ContentType, "application/json;odata=verbose" }, { "X-RequestDigest", formDigestValue },
            };
        }

        public static WebHeaderCollection GetDeleteHeaders(string formDigestValue)
        {
            return new WebHeaderCollection
            {
                { "X-HTTP-Method", "DELETE" }, { "IF-MATCH", "*" }, { HttpRequestHeader.ContentType, "application/json;odata=verbose" }, { "X-RequestDigest", formDigestValue },
            };
        }
    }
}