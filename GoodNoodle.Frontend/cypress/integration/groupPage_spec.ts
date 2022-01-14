describe('Group page tests', () => {
  before(() => {
    cy.visit('/register');

    cy.get('#input-name').type('Cypress Login');
    cy.get('#input-email').type('cypress@login.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('#input-re-password').type('Cypress1#');
    cy.get('.custom-checkbox').click();
    cy.get('button[status="primary"]').contains('Register').click();
  });

  it('Should make user status accepted', () => {
    cy.visit('/login');

    cy.get('#input-email').type('goodnoodle.noreply@gmail.com');
    cy.get('#input-password').type('!Password123#');
    cy.get('button[status="primary"]').contains('Log In').click();

    cy.get('.user-name').contains('Admin').should('be.visible');
    cy.visit('/admin');

    cy.get('#edit-cypresslogincom').click();
    cy.get('#user-status').click();
    cy.get('nb-option[value="1"]').click();
    cy.get('#save-user').click();
  });

  it('Should login as accepted user and create group', () => {
    cy.visit('/login');

    cy.get('#input-email').type('cypress@login.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('button[status="primary"]').contains('Log In').click();

    cy.get('#create-group-btn').click();

    // Cypress is too fast so we need to wait before typing
    cy.wait(500);
    cy.get('#group-name').type('CypressGroup');
    cy.get('#submit-group').click();
  });

  it('Should have logged in user and go to group', () => {
    cy.visit('/login');

    cy.get('#input-email').type('cypress@login.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('button[status="primary"]').contains('Log In').click();

    // api request is not as fast as cypress
    cy.wait(1000);

    cy.get('button').contains('CypressGroup').click();

    cy.get("nb-icon[ng-reflect-config='briefcase-outline']").click();

    cy.get('nb-user').contains('cypress@login.com').should('be.visible');
  });
});
