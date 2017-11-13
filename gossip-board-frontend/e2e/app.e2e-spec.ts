import { GossipBoardUiTemplatePage } from './app.po';

describe('gossip-board-ui-template App', () => {
  let page: GossipBoardUiTemplatePage;

  beforeEach(() => {
    page = new GossipBoardUiTemplatePage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
