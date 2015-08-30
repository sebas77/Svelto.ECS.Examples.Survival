namespace Svelto.Command
{
	internal interface IInjectableCommand : ICommand
	{
	    ICommand Inject(object dependency);
	}
	
	internal interface IMultiInjectableCommand : ICommand
	{
	    ICommand Inject(params object[] notification);
	}
}
