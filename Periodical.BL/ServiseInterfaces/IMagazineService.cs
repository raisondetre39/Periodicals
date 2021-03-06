﻿using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Collections.Generic;

namespace Periodical.BL.ServiseInterfaces
{
    public interface IMagazineService
    {
        OperationStatus Create(MagazineDTO magazineDTO, int authorId, int[] tags);

        OperationStatus Edit(MagazineDTO magazineDTO, int authorId, int[] tags);

        MagazineDTO GetById(int? id);

        IEnumerable<MagazineDTO> GetAll();

        IEnumerable<MagazineDTO> GetBy(string name);

        IEnumerable<MagazineDTO> GetAuthorMagazines(int id);

        OperationStatus Delete(int? id);
    }
}
