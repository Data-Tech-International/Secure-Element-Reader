using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Enums
{
    public enum SubmitMessages 
    {
        SuccessSubmit,
        UnableToSubmit,
        CantReadInternal
    }
}

//App.Current.TryFindResource("SuccessSubmit", out var resultSuccess);
//App.Current.TryFindResource("UnableToSubmit", out var resultUnable);
//App.Current.TryFindResource("CantReadInternal", out var resultCant);
