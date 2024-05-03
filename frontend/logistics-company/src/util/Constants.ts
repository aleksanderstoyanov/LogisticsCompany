/**
 * The default URLs for different endpoints
 * @constant
 */
export const API_URL = "https://localhost:7209/api";

/**
 * The unique form identifiers
 * @constant
 */

export const PACKAGE_FORM_IDS = ["address", "to", "weight", "userId", "toOffice"];
export const LOGIN_FORM_IDS = ["email", "password"];
export const REGISTER_FORM_IDS = ["user", "firstName", "lastName", "email", "password"];

/**
 * The unique date picker identifiers
 */
export const START_PERIOD_ID = "StartPeriod";
export const END_PERIOD_ID = "EndPeriod";

/**
 * The default values for different models.
 * @constant
 */
export const DEFAULT_USER_EMAIL = "None";
export const DEFAULT_USER_Role = "Anonymous";
export const DEFAULT_USER_ID = 0;

export const DEFAULT_PACKAGE_ADDRESS = "None";
export const DEFAULT_PACKAGE_FROM_ID = 0;
export const DEFAULT_PACKAGE_TO_ID = 0;
export const DEFAULT_PACKAGE_DELIVERY_ID= 0;
export const DEFAULT_PACKAGE_WEIGHT = 0;
export const DEFAULT_PACKAGE_TO_OFFICE = false;

export const DEFAULT_USER_USERNAME = "None";
export const DEFAULT_USER_PASSWORD = "123";

/**
 * The roles for Users.
 * @constant
 */
export const USER_ROLES = ['Client', 'OfficeEmployee', 'Courier'];

/**
 * The default package statuses based on Courier
 * or OfficeEmployee roles.
 * @constant
 */
export const PACKAGE_STATUSES = {
    "Courier": ["InDelivery", "Delivered"],
    "OfficeEmployee": ["NonRegistered", "Registered"]
};

/**
 * The default style properties for different components.
 * @constant
 */
export const MODAL_STYLE = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 500,
    bgcolor: 'background.paper',
    border: '2px solid #000',
    boxShadow: 24,
    pt: 2,
    px: 4,
    pb: 3,
};

export const GRID_BOX_STYLE = {
    height: 400,
    width: '100%',
    marginTop: "7%"
};

export const GRID_CARD_CONTAINER_STYLE = {
    direction: "row",
    justifyContent: "center",
    alignItems: "center"
};