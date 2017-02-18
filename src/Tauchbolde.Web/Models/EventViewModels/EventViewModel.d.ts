declare module server {
	interface eventViewModel {
		event: {
			id: any;
			name: string;
			organisator: {
				additionalUserInfos: {
					id: any;
					websiteUrl: string;
					twitterHandle: string;
					skypeId: string;
					slogan: string;
					education: string;
					experience: string;
					mobilePhone: string;
					notificationIntervalInHours: number;
					userId: string;
					user: any;
				};
				notificationses: any[];
				events: any[];
				posts: any[];
				comments: any[];
			};
			location: string;
			meetingPoint: string;
			description: string;
			startTime: Date;
			endTime: Date;
			canceled: boolean;
			deleted: boolean;
			participants: any[];
			comments: any[];
			startEndTimeAsString: string;
		};
		buddyTeamNames: string[];
	}
}