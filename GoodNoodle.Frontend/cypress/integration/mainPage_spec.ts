describe('Main page Tests', () => {
  it('Should visit the initial project page', () => {
    cy.visit('/');
    cy.contains('The GoodNoodleBoard');
  });
});
