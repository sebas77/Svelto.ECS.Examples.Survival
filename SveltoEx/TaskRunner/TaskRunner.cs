using System.Collections;
using Svelto.Tasks;
using Svelto.Tasks.Internal;
using System;

public class TaskRunner
{
	static TaskRunner _instance;
    
    static public TaskRunner Instance
	{
		get 
		{
			if (_instance == null)
				InitInstance();
			
			return _instance;
		}
	}
		
	public void Run(IEnumerable task)
	{
		if (task == null)
			return;

        Run(task.GetEnumerator());
	}

    public void Run(TaskCollection task)
	{
		if (task == null)
			return;

		_runner.StartCoroutine(task.GetEnumerator());
	}

    public void Run(IEnumerator task)
	{
		if (task == null)
			return;

         _runner.StartCoroutine(new SingleTask(task));
	}

	public void RunSync(IEnumerator task)
	{
		if (task == null)
			return;

		IEnumerator taskToRun = new SingleTask(task);
		
		while (taskToRun.MoveNext() == true);
	}

    public void RunSync(IEnumerable task)
	{
		if (task == null)
			return;

		RunSync(task);
	}

	public TaskRoutine CreateTask(IEnumerable task)
	{
		if (task == null)
			return null;

		return CreateTask(task.GetEnumerator);
	}

	public TaskRoutine CreateTask(Func<IEnumerator> taskGenerator)
	{
		PausableTask ptask = new PausableTask(_runner);
		
		return new TaskRoutine(ptask, taskGenerator);
	}

    public TaskRoutine CreateEmptyTask()
	{
        return _taskRoutinePool.RetrieveTask();
	}

	public void PauseManaged()
	{
		_runner.paused = true;
	}
	
	public void ResumeManaged()
	{
		_runner.paused = false;
	}
	
	public void Stop()
	{
		if (_runner != null)
			_runner.StopAllCoroutines();
	}
	
	static void InitInstance()
	{
		_instance 			= new TaskRunner();
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
		_instance._runner 	= new MonoRunner();
#else
        _instance._runner = new MultiThreadRunner();
#endif
        _instance._taskRoutinePool = new TaskRoutinePool(_instance._runner);
	}

    TaskRoutinePool _taskRoutinePool;
    IRunner         _runner;
}
	



 
