using System;
using UnityEngine.Profiling;

namespace Svelto.Common
{
#if UNITY_5_3_OR_NEWER && ENABLE_PLATFORM_PROFILER
    
#if !PROFILER
#warning with the compiler directive PROFILER disabled, the profiling will be skewed by the operations Svelto performs for debugging purposes, to avoid it, enable the compiler directive PROFILER
#endif    
    public struct PlatformProfilerMT : IDisposable
    {
        readonly CustomSampler sampler;

        public PlatformProfilerMT(string name):this()
        {
            Profiler.BeginThreadProfiling("Svelto.Tasks", name);
        }

        public void Dispose()
        {
            Profiler.EndThreadProfiling();
        }

        public DisposableStruct Sample(string samplerName, string samplerInfo = null)
        {
            return new DisposableStruct(CustomSampler.Create(samplerInfo != null ? samplerName.FastConcat(" ", samplerInfo) : samplerName));
        }

        public struct DisposableStruct : IDisposable
        {
            readonly CustomSampler _sampler;

            public DisposableStruct(CustomSampler customSampler)
            {
                _sampler = customSampler;
                _sampler.Begin();
            }

            public void Dispose()
            {
                _sampler.End();
            }
        }
    }
    
    public struct PlatformProfiler : IDisposable
    {
        public PlatformProfiler(string name) : this()
        {
            Profiler.BeginSample(name);
        }

        public void Dispose()
        {
            Profiler.EndSample();
        }

        public DisposableStruct Sample(string samplerName, string samplerInfo = null)
        {
            return new DisposableStruct(samplerInfo != null ? samplerName.FastConcat(" ", samplerInfo) : samplerName);
        }

        public struct DisposableStruct : IDisposable
        {
            public DisposableStruct(string samplerName)
            {
                Profiler.BeginSample(samplerName);
            }

            public void Dispose()
            {
                Profiler.EndSample();
            }
        }
    }
#else
    public struct PlatformProfiler : IDisposable
    {
        public PlatformProfiler(string name)
        {}

        public void Dispose()
        {}

        public DisposableStruct Sample(string samplerName, string sampleInfo = null)
        {
            return new DisposableStruct();
        }
        
        public DisposableStruct Sample(string samplerName)
        {
            return new DisposableStruct();
        }

        public struct DisposableStruct : IDisposable
        {
            public void Dispose()
            {}
        }
    }
    
    public struct PlatformProfilerMT : IDisposable
    {
        public PlatformProfilerMT(string name)
        {}

        public void Dispose()
        {}

        public DisposableStruct Sample(string samplerName)
        {
            return new DisposableStruct();
        }

        public struct DisposableStruct : IDisposable
        {
            public void Dispose()
            {}
        }
    }
#endif    
}