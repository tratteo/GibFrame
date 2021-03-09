// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : com.tratteo.gibframe.Packages.com.tratteo.gibframe.Runtime.Scripts.Patterns : Query.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;

namespace GibFrame.Patterns
{
    public class Query<Result> : IEquatable<Query<Result>>
    {
        private event Func<Result> Provider;

        public void SubscribeProvider(Func<Result> Provider)
        {
            this.Provider = Provider;
        }

        public void UsubscribeProvider()
        {
            foreach (Delegate del in Provider.GetInvocationList())
            {
                Provider -= del as Func<Result>;
            }
        }

        public Result Obtain()
        {
            if (Provider != null)
            {
                return Provider.Invoke();
            }
            return default;
        }

        public bool Equals(Query<Result> other)
        {
            return Provider.Equals(other.Provider);
        }
    }

    public class Query<A, Result> : IEquatable<Query<A, Result>>
    {
        private event Func<A, Result> Provider;

        public void SubscribeProvider(Func<A, Result> Provider)
        {
            this.Provider = Provider;
        }

        public void UsubscribeProvider()
        {
            foreach (Delegate del in Provider.GetInvocationList())
            {
                Provider -= del as Func<A, Result>;
            }
        }

        public Result Obtain(A arg)
        {
            if (Provider != null)
            {
                return Provider.Invoke(arg);
            }
            return default;
        }

        public bool Equals(Query<A, Result> other)
        {
            return Provider.Equals(other.Provider);
        }
    }

    public class Query<A1, A2, Result> : IEquatable<Query<A1, A2, Result>>
    {
        private event Func<A1, A2, Result> Provider;

        public void SubscribeProvider(Func<A1, A2, Result> Provider)
        {
            this.Provider = Provider;
        }

        public void UsubscribeProvider()
        {
            foreach (Delegate del in Provider.GetInvocationList())
            {
                Provider -= del as Func<A1, A2, Result>;
            }
        }

        public Result Obtain(A1 arg1, A2 arg2)
        {
            if (Provider != null)
            {
                return Provider.Invoke(arg1, arg2);
            }
            return default;
        }

        public bool Equals(Query<A1, A2, Result> other)
        {
            return Provider.Equals(other.Provider);
        }
    }

    public class MulticastQuery<Result>
    {
        private List<(object sender, Query<Result> query)> Providers;

        public MulticastQuery()
        {
            Providers = new List<(object sender, Query<Result>)>();
        }

        public void SubscribeProvider(object sender, Func<Result> Provider)
        {
            Query<Result> query = new Query<Result>();
            query.SubscribeProvider(Provider);
            Providers.Add((sender, query));
        }

        public void Clear()
        {
            Providers.Clear();
        }

        public void UsubscribeProvider(object sender, Func<Result> Provider)
        {
            Query<Result> del = new Query<Result>();
            del.SubscribeProvider(Provider);
            Providers.RemoveAll((e) => e.sender.Equals(sender) && del.Equals(e.query));
        }

        public (object sender, Result result)[] Obtain()
        {
            int count = Providers.Count;
            (object sender, Result result)[] results = new (object sender, Result result)[count];
            for (int i = 0; i < count; i++)
            {
                results[i] = (Providers[i].sender, Providers[i].query.Obtain());
            }
            return results;
        }
    }

    public class MulticastQuery<A, Result>
    {
        private List<(object sender, Query<A, Result> query)> Providers;

        public MulticastQuery()
        {
            Providers = new List<(object sender, Query<A, Result> query)>();
        }

        public void Clear()
        {
            Providers.Clear();
        }

        public void SubscribeProvider(object sender, Func<A, Result> Provider)
        {
            Query<A, Result> query = new Query<A, Result>();
            query.SubscribeProvider(Provider);
            Providers.Add((sender, query));
        }

        public void UsubscribeProvider(object sender, Func<A, Result> Provider)
        {
            Query<A, Result> del = new Query<A, Result>();
            del.SubscribeProvider(Provider);
            Providers.RemoveAll((e) => e.sender.Equals(sender) && del.Equals(e.query));
        }

        public (object sender, Result result)[] Obtain(A arg)
        {
            int count = Providers.Count;
            (object sender, Result result)[] results = new (object sender, Result result)[count];
            for (int i = 0; i < count; i++)
            {
                (object sender, Query<A, Result> query) current = Providers[i];
                results[i] = (current.sender, current.query.Obtain(arg));
            }
            return results;
        }
    }

    public class MulticastQuery<A1, A2, Result>
    {
        private List<(object sender, Query<A1, A2, Result> query)> Providers;

        public MulticastQuery()
        {
            Providers = new List<(object sender, Query<A1, A2, Result> query)>();
        }

        public void Clear()
        {
            Providers.Clear();
        }

        public void SubscribeProvider(object sender, Func<A1, A2, Result> Provider)
        {
            Query<A1, A2, Result> query = new Query<A1, A2, Result>();
            query.SubscribeProvider(Provider);
            Providers.Add((sender, query));
        }

        public void UsubscribeProvider(object sender, Func<A1, A2, Result> Provider)
        {
            Query<A1, A2, Result> del = new Query<A1, A2, Result>();
            del.SubscribeProvider(Provider);
            Providers.RemoveAll((e) => e.sender.Equals(sender) && del.Equals(e.query));
        }

        public (object sender, Result result)[] Obtain(A1 arg1, A2 arg2)
        {
            int count = Providers.Count;
            (object sender, Result result)[] results = new (object sender, Result result)[count];
            for (int i = 0; i < count; i++)
            {
                (object sender, Query<A1, A2, Result> query) current = Providers[i];
                results[i] = (current.sender, current.query.Obtain(arg1, arg2));
            }
            return results;
        }
    }
}
