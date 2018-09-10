using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicBlackJack.BusinessLogic.Interface
{
    interface IAutomatState
    {
        String FirstCardCheck();
        String PlayerTakeCardAndCheck();
        String DillerTakeCardAndCheck();
        String PlayerAndDillerScoringCard();
    }

    public interface IAutomat
}
