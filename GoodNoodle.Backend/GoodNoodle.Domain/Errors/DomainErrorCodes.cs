namespace GoodNoodle.Domain.Errors;

public static class DomainErrorCodes
{
    // General
    public const string NotFound = "ENTITY_NOT_FOUND";

    // Group
    public const string GroupEmptyId = "GROUP_ID_MAY_NOT_BE_EMPTY";
    public const string GroupEmptyName = "GROUP_NAME_MAY_NOT_BE_EMPTY";
    public const string GroupTooLongName = "GROUP_NAME_IS_TOO_LONG";
    public const string GroupEmptyImage = "GROUP_IMAGE_MAY_NOT_BE_EMPTY";
    public const string GroupImageNotBase64 = "GROUP_IMAGE_MUST_BE_BASE64";
    public const string GroupAlreadyExist = "GROUP_ALREADY_EXISTS";

    // User
    public const string UserEmptyPassword = "USER_PASSWORD_MAY_NOT_BE_EMPTY";
    public const string UserShortPassword = "USER_PASSWORD_MAY_NOT_BE_SHORTER_THAN_6_CHARACTERS";
    public const string UserLongPassword = "USER_PASSWORD_MAY_NOT_BE_LONGER_THAN_50_CHARACTERS";
    public const string UserUppercaseLetterPassword = "USER_PASSWORD_MUST_CONTAIN_A_UPPERCASE_LETTER";
    public const string UserLowercaseLetterPassword = "USER_PASSWORD_MUST_CONTAIN_A_LOWERCASE_LETTER";
    public const string UserNumberPassword = "USER_PASSWORD_MUST_CONTAIN_A_NUMBER";
    public const string UserSpecialCharPassword = "USER_PASSWORD_MUST_CONTAIN_A_SPECIAL_CHARACTER";
    public const string UserOldPasswordNotTheSame = "USER_OLD_PASSWORD_IS_NOT_CORRECT";
    public const string UserPasswordIsIncorrect = "USER_PASSWORD_IS_NOT_CORRECT";
    public const string UserEmptyFullName = "USER_FULL_NAME_MAY_NOT_BE_EMPTY";
    public const string UserLongFullName = "USER_FULL_NAME_MAY_NOT_BE_LONGER_THAN_100_CHARACTERS";
    public const string UserEmptyId = "USER_ID_MAY_NOT_BE_EMPTY";
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
    public const string UserInvalidEmail = "USER_EMAIL_IS_INVALID";
    public const string UserEmptyEmail = "USER_EMAIL_MAY_NOT_BE_EMPTY";
    public const string UserIsNotAuthorized = "USER_MUST_BE_AUTHORIZED";
    public const string UserHasNoPermissions = "USER_MUST_HAS_PERMISSIONS";

    // User in group
    public const string UserInGroupIdEmpty = "USER_IN_GROUP_ID_MAY_NOT_BE_EMPTY";
    public const string UserInGroupUserIdEmpty = "USER_IN_GROUP_USER_ID_MAY_NOT_BE_EMPTY";
    public const string UserInGroupGroupIdEmpty = "USER_IN_GROUP_GROUP_ID_MAY_NOT_BE_EMPTY";
    public const string UserInGroupAlreadyExists = "USER_IN_GROUP_ALREADY_EXISTS";
    public const string UserInGroupRoleNotExist = "USER_IN_GROUP_ROLE_DOES_NOT_EXIST";

    // Star
    public const string StarEmptyId = "STAR_ID_MAY_NOT_BE_EMPTY";
    public const string StarEmptyUserId = "STAR_USER_ID_MAY_NOT_BE_EMPTY";
    public const string StarEmptyGroupId = "STAR_GROUP_ID_MAY_NOT_BE_EMPTY";
    public const string StarEmptyReason = "STAR_REASON_MAY_NOT_BE_EMPTY";
    public const string StarLongReason = "STAR_REASON_MAY_NOT_BE_LONGER_THAN_1000_CHARACTERS";
    public const string StarIdAlreadyExists = "STAR_ID_IS_ALREADY_TAKEN";

    // Invitations
    public const string InvitationEmptyId = "INVITATION_ID_MAY_MOT_BE_EMPTY";
}
