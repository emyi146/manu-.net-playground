



var param = new CreateParameter();

var casParam = (IActionParameter)param;


var createAction = new CreateAction<CreateParameter>();

//var castToInterface = (IAction<IActionParameter>)createAction;
var castAsInterface = createAction as IAction<IActionParameter>;

Console.WriteLine("success");



public interface IAction<TParameters> where TParameters : IActionParameter
{

}

public interface IActionParameter { }



public class CreateAction<TParameter> : IAction<TParameter> where TParameter : IActionParameter
{
}

public class CreateParameter : IActionParameter { }


