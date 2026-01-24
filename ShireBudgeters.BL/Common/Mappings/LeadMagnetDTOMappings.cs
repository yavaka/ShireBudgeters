using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.BL.Common.Mappings;

internal static class LeadMagnetDTOMappings
{
    /// <summary>
    /// Maps a LeadMagnetModel to a LeadMagnetDTO.
    /// </summary>
    public static LeadMagnetDTO ToLeadMagnetDTO(this LeadMagnetModel leadMagnet)
    {
        return new LeadMagnetDTO
        {
            Id = leadMagnet.Id,
            CategoryId = leadMagnet.CategoryId,
            Title = leadMagnet.Title,
            FormActionUrl = leadMagnet.FormActionUrl,
            DownloadFileUrl = leadMagnet.DownloadFileUrl,
            IsActive = leadMagnet.IsActive,
            CreatedBy = leadMagnet.CreatedBy,
            CreatedDate = leadMagnet.CreatedDate,
            ModifiedBy = leadMagnet.ModifiedBy,
            ModifiedDate = leadMagnet.ModifiedDate
        };
    }

    /// <summary>
    /// Maps a LeadMagnetDTO to a LeadMagnetModel for create/update operations.
    /// </summary>
    public static LeadMagnetModel ToLeadMagnetModel(this LeadMagnetDTO leadMagnetDto)
    {
        return new LeadMagnetModel
        {
            Id = leadMagnetDto.Id,
            CategoryId = leadMagnetDto.CategoryId,
            Title = leadMagnetDto.Title,
            FormActionUrl = leadMagnetDto.FormActionUrl,
            DownloadFileUrl = leadMagnetDto.DownloadFileUrl,
            IsActive = leadMagnetDto.IsActive,
            CreatedBy = leadMagnetDto.CreatedBy,
            CreatedDate = leadMagnetDto.CreatedDate,
            ModifiedBy = leadMagnetDto.ModifiedBy,
            ModifiedDate = leadMagnetDto.ModifiedDate
        };
    }
}

