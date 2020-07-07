#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
namespace DeveloperTestInterfaces
{
    public interface IDeveloperTest
    {
        void RunQuestionOne(ICharacterReader reader, IOutputResult output);
        void RunQuestionTwo(ICharacterReader[] readers, IOutputResult output);
    }
}