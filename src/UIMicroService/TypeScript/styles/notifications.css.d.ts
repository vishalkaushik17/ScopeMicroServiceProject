declare namespace NotificationsCssNamespace {
  export interface INotificationsCss {
    "toastr-class": string;
  }
}

declare const NotificationsCssModule: NotificationsCssNamespace.INotificationsCss & {
  /** WARNING: Only available when `css-loader` is used without `style-loader` or `mini-css-extract-plugin` */
  locals: NotificationsCssNamespace.INotificationsCss;
};

export = NotificationsCssModule;
