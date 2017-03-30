﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using CatLib.API.FilterChain;
using System.Collections.Generic;
using System;

namespace CatLib.FilterChain
{

    public class FilterChain : IFilterChain
    {

        public IFilterChain<TIn> Create<TIn>()
        {
            return new FilterChain<TIn>();
        }

        public IFilterChain<TIn, TOut> Create<TIn, TOut>()
        {
            return new FilterChain<TIn, TOut>();
        }

        public IFilterChain<TIn, TOut, TException> Create<TIn, TOut, TException>()
        {
            return new FilterChain<TIn, TOut, TException>();
        }

    }

    public class FilterChain<TIn> : IFilterChain<TIn>
    {

        protected class FilterChainWrapper : IFilter<TIn>
        {
            protected Action<TIn, IFilterChain<TIn>> filter;

            public FilterChainWrapper(Action<TIn, IFilterChain<TIn>> filter)
            {
                this.filter = filter;
            }

            public void Do(TIn inData, IFilterChain<TIn> chain)
            {
                filter(inData, chain);
            }
        }

        protected List<IFilter<TIn>> filterList = new List<IFilter<TIn>>();

        public IFilter<TIn>[] FilterList { get { return filterList.ToArray(); } }

        protected Action<TIn> then;

        protected int index = 0;

        public IFilterChain<TIn> Add(Action<TIn, IFilterChain<TIn>> filter)
        {
            return Add(new FilterChainWrapper(filter));
        }

        public IFilterChain<TIn> Add(IFilter<TIn> filter)
        {
            filterList.Add(filter);
            return this;
        }

        public IFilterChain<TIn> Then(Action<TIn> then)
        {
            this.then = then;
            return this;
        }

        public void Do(TIn inData)
        {
            if (index >= filterList.Count)
            {
                if (then != null) { then.Invoke(inData); }
                return;
            }
            filterList[index++].Do(inData , this);
        }

    }

    public class FilterChain<TIn, TOut> : IFilterChain<TIn, TOut>
    {

        protected class FilterChainWrapper : IFilter<TIn, TOut>
        {
            protected Action<TIn, TOut, IFilterChain<TIn, TOut>> filter;

            public FilterChainWrapper(Action<TIn, TOut, IFilterChain<TIn, TOut>> filter)
            {
                this.filter = filter;
            }

            public void Do(TIn inData , TOut outData, IFilterChain<TIn, TOut> chain)
            {
                filter(inData, outData, chain);
            }
        }

        protected List<IFilter<TIn, TOut>> filterList = new List<IFilter<TIn, TOut>>();

        public IFilter<TIn, TOut>[] FilterList { get { return filterList.ToArray(); } }

        protected Action<TIn , TOut> then;

        protected int index = 0;

        public IFilterChain<TIn, TOut> Add(Action<TIn , TOut, IFilterChain<TIn, TOut>> filter)
        {
            return Add(new FilterChainWrapper(filter));
        }

        public IFilterChain<TIn, TOut> Add(IFilter<TIn, TOut> filter)
        {
            filterList.Add(filter);
            return this;
        }

        public IFilterChain<TIn , TOut> Then(Action<TIn , TOut> then)
        {
            this.then = then;
            return this;
        }

        public void Do(TIn inData, TOut outData)
        {
            if (index >= filterList.Count)
            {
                if (then != null) { then.Invoke(inData , outData); }
                return;
            }
            filterList[index++].Do(inData , outData , this);
        }

    }


    public class FilterChain<TIn, TOut,TException> : IFilterChain<TIn, TOut, TException>
    {

        protected class FilterChainWrapper : IFilter<TIn, TOut, TException>
        {
            protected Action<TIn, TOut, TException, IFilterChain<TIn, TOut , TException>> filter;

            public FilterChainWrapper(Action<TIn, TOut, TException, IFilterChain<TIn, TOut , TException>> filter)
            {
                this.filter = filter;
            }

            public void Do(TIn inData , TOut outData, TException exception, IFilterChain<TIn, TOut , TException> chain)
            {
                filter(inData, outData , exception, chain);
            }
        }

        protected List<IFilter<TIn, TOut, TException>> filterList = new List<IFilter<TIn, TOut, TException>>();

        public IFilter<TIn, TOut, TException>[] FilterList { get { return filterList.ToArray(); } }

        protected Action<TIn , TOut, TException> then;

        protected int index = 0;

        public IFilterChain<TIn, TOut , TException> Add(Action<TIn, TOut, TException, IFilterChain<TIn, TOut , TException>> filter)
        {
            return Add(new FilterChainWrapper(filter));
        }

        public IFilterChain<TIn, TOut, TException> Add(IFilter<TIn, TOut, TException> filter)
        {
            filterList.Add(filter);
            return this;
        }

        public IFilterChain<TIn , TOut, TException> Then(Action<TIn , TOut, TException> then)
        {
            this.then = then;
            return this;
        }

        public void Do(TIn inData, TOut outData, TException exception)
        {
            if (index >= filterList.Count)
            {
                if (then != null) { then.Invoke(inData , outData , exception); }
                return;
            }
            filterList[index++].Do(inData , outData ,exception , this);
        }

    }

        
}
