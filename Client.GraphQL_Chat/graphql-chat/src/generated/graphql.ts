import { gql } from 'apollo-angular';
import { Injectable } from '@angular/core';
import * as Apollo from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
export type AddMessageMutationVariables = Exact<{
  fromId: Scalars['String']['input'];
  content: Scalars['String']['input'];
  sentAt: Scalars['DateTimeOffset']['input'];
}>;


export type AddMessageMutation = { __typename?: 'ChatMutation', addMessage?: { __typename?: 'Message', content: string, sentAt: any, from?: { __typename?: 'MessageFrom', id: string, displayName: string } | null } | null };

export type MessageAddedSubscriptionVariables = Exact<{ [key: string]: never; }>;


export type MessageAddedSubscription = { __typename?: 'ChatSubscription', messageAdded?: { __typename?: 'Message', content: string, sentAt: any, from?: { __typename?: 'MessageFrom', id: string } | null } | null };

/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  /** The `DateTime` scalar type represents a date and time. `DateTime` expects timestamps to be formatted in accordance with the [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard. */
  DateTime: { input: any; output: any; }
  /** The `DateTimeOffset` scalar type represents a date, time and offset from UTC. `DateTimeOffset` expects timestamps to be formatted in accordance with the [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard. */
  DateTimeOffset: { input: any; output: any; }
};

export type ChatMutation = {
  __typename?: 'ChatMutation';
  addMessage?: Maybe<Message>;
};


export type ChatMutationAddMessageArgs = {
  message?: InputMaybe<MessageInput>;
};

export type ChatQuery = {
  __typename?: 'ChatQuery';
  messages?: Maybe<Array<Maybe<Message>>>;
};

export type ChatSubscription = {
  __typename?: 'ChatSubscription';
  messageAdded?: Maybe<Message>;
  messageAddedAsync?: Maybe<Message>;
  messageAddedByUser?: Maybe<Message>;
  messageAddedByUserAsync?: Maybe<Message>;
  messageCounter?: Maybe<Scalars['Int']['output']>;
  messageGetAll?: Maybe<Array<Maybe<Message>>>;
  newMessageContent?: Maybe<Scalars['String']['output']>;
};


export type ChatSubscriptionMessageAddedByUserArgs = {
  id: Scalars['String']['input'];
};


export type ChatSubscriptionMessageAddedByUserAsyncArgs = {
  id: Scalars['String']['input'];
};

export type Message = {
  __typename?: 'Message';
  content: Scalars['String']['output'];
  from?: Maybe<MessageFrom>;
  sentAt: Scalars['DateTime']['output'];
};

export type MessageFrom = {
  __typename?: 'MessageFrom';
  displayName: Scalars['String']['output'];
  id: Scalars['String']['output'];
};

export type MessageInput = {
  content?: InputMaybe<Scalars['String']['input']>;
  fromId?: InputMaybe<Scalars['String']['input']>;
  sentAt?: InputMaybe<Scalars['DateTimeOffset']['input']>;
};

export const AddMessageDocument = gql`
    mutation AddMessage($fromId: String!, $content: String!, $sentAt: DateTimeOffset!) {
  addMessage(message: {fromId: $fromId, content: $content, sentAt: $sentAt}) {
    content
    sentAt
    from {
      id
      displayName
    }
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class AddMessageGQL extends Apollo.Mutation<AddMessageMutation, AddMessageMutationVariables> {
    document = AddMessageDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }
export const MessageAddedDocument = gql`
    subscription MessageAdded {
  messageAdded {
    content
    sentAt
    from {
      id
    }
  }
}
    `;

  @Injectable({
    providedIn: 'root'
  })
  export class MessageAddedGQL extends Apollo.Subscription<MessageAddedSubscription, MessageAddedSubscriptionVariables> {
    document = MessageAddedDocument;
    
    constructor(apollo: Apollo.Apollo) {
      super(apollo);
    }
  }