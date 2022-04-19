using System;

namespace Game.Resource
{
    public class AssetsRequest
    {
        public string path;

        public RequestStatus status;

        public Action<AssetsResponse> callback;

        public Action fail;
    }

    public enum RequestStatus
    {
        Ready,
        Loading,
        Complete,
    }
}